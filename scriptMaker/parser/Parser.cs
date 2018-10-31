using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using System.Reflection;

namespace scriptMaker.parser
{
    class Parser
    {
        protected abstract class Element
        {
            public abstract void parse(Lexer lexer, List<ASTree> res);
            public abstract bool match(Lexer lexer);
        }

        protected class Tree : Element
        {
            protected Parser parser;
            public Tree(Parser p) { parser = p; }
            public override void parse(Lexer lexer, List<ASTree> res)
            {
                res.Add(parser.parse(lexer));
            }
            public override bool match(Lexer lexer)
            {
                return parser.match(lexer);
            }
        }


        protected class OrTree : Element
        {
            protected Parser[] parsers;
            public OrTree(Parser[] p) { parsers = p; }
            public override void parse(Lexer lexer, List<ASTree> res)
            {
                Parser p = choose(lexer);
                if (p == null)
                    throw new ParseException(lexer.peek(0).ToString());
                else
                    res.Add(p.parse(lexer));
            }
            public override bool match(Lexer lexer)
            {
                return choose(lexer) != null;
            }
            public Parser choose(Lexer lexer)
            {
                foreach (Parser p in parsers)
                    if (p.match(lexer))
                        return p;

                return null;
            }
            public void insert(Parser p)
            {
                Parser[] newParsers = new Parser[parsers.Length + 1];
                newParsers[0] = p;
                System.Array.Copy(parsers, 0, newParsers, 1, parsers.Length);
                parsers = newParsers;
            }
        }

        protected class Repeat : Element
        {
            protected Parser parser;
            protected bool onlyOnce;
            public Repeat(Parser p, bool once) { parser = p; onlyOnce = once; }
            public override void parse(Lexer lexer, List<ASTree> res)
            {
                while (parser.match(lexer))
                {
                    ASTree t = parser.parse(lexer);
                    if (t.GetType() != typeof(ASTList) || t.numChildren() > 0)
                        res.Add(t);
                    if (onlyOnce)
                        break;
                }
            }
            public override bool match(Lexer lexer)
            {
                return parser.match(lexer);
            }
        }

        protected abstract class AToken : Element
        {
            protected Factory factory;
            protected AToken(System.Type type)
            {
                if (type == null)
                    type = typeof(ASTLeaf);
                factory = Factory.get(type, typeof(Token));
            }

            public override void parse(Lexer lexer, List<ASTree> res)
            {
                Token t = lexer.read();
                if (test(t))
                {
                    ASTree leaf = factory.make(t);
                    res.Add(leaf);
                }
                else
                    throw new ParseException("");
            }
            public override bool match(Lexer lexer) 
            {
                return test(lexer.peek(0));
            }
            protected abstract bool test(Token t);
        }


        protected class IdToken : AToken
        {
            HashSet<String> reserved;
            public IdToken(System.Type type, HashSet<String> r):base(type)
            {
                reserved = r != null ? r : new HashSet<String>();
            }
            protected override bool test(Token t)
            {
                return t.isIdentifier() && !reserved.Contains(t.getText());
            }
        }

        protected class NumToken : AToken
        {
        public NumToken(System.Type type): base(type) {  }
        protected override bool test(Token t) { return t.isNumber(); }
        }

        protected class StrToken : AToken
        {
            public StrToken(System.Type type) : base(type) { }
            protected override bool test(Token t) { return t.isString(); }
        }

        protected class Leaf : Element
        {
            protected String[] tokens;
            public Leaf(String[] pat) { tokens = pat; }
            public override void parse(Lexer lexer, List<ASTree> res)
            {
                Token t = lexer.read();
                if (t.isIdentifier())
                    foreach (String token in tokens)
                        if (token.Equals(t.getText()))
                        {
                            find(res, t);
                            return;
                        }

                if (tokens.Length > 0)
                    throw new ParseException(tokens[0] + " expected." + t);
                else
                    throw new ParseException("");
            }
            protected void find(List<ASTree> res, Token t)
            {
                res.Add(new ASTLeaf(t));
            }
            public override bool match(Lexer lexer)
            {
                Token t = lexer.peek(0);
                if (t.isIdentifier())
                    foreach (String token in tokens)
                        if (token.Equals(t.getText()))
                            return true;

                return false;
            }
        }


        protected class Skip : Leaf
        {
            public Skip(String[] t):base(t) {  }
            new protected void find(List<ASTree> res, Token t) { }
       }

        public class Precedence
        {
            public int value;
            public bool leftAssoc; // left associative
            public Precedence(int v, bool a)
            {
                value = v; leftAssoc = a;
            }
        }

        public class Operators : Dictionary<String, Precedence>
        {
            public static bool LEFT = true;
            public static bool RIGHT = false;
            public void add(String name, int prec, bool leftAssoc)
            {
                Add(name, new Precedence(prec, leftAssoc));
            }
        }



        protected class Expr : Element
        {
            protected Factory factory;
            protected Operators ops;
            protected Parser factor;
            public Expr(System.Type clazz, Parser exp,
                           Operators map)
            {
                factory = Factory.getForASTList(clazz);
                ops = map;
                factor = exp;
            }
            private Precedence nextOperator(Lexer lexer)
            {
                Token t = lexer.peek(0);
                Precedence result = null;
                if (t.isIdentifier())
                {
                    ops.TryGetValue(t.getText(), out result);
                }
                return result;
            }
            private static bool rightIsExpr(int prec, Precedence nextPrec)
            {
                if (nextPrec.leftAssoc)
                    return prec < nextPrec.value;
                else
                    return prec <= nextPrec.value;
            }
            public override bool match(Lexer lexer)
            {
                return factor.match(lexer);
            }
            public override void parse(Lexer lexer, List<ASTree> res)
            {
                ASTree right = factor.parse(lexer);
                Precedence prec;
                while ((prec = nextOperator(lexer)) != null)
                    right = doShift(lexer, right, prec.value);

                res.Add(right);
            }
            private ASTree doShift(Lexer lexer, ASTree left, int prec)
            {
                List<ASTree> list = new List<ASTree>();
                list.Add(left);
                list.Add(new ASTLeaf(lexer.read()));
                ASTree right = factor.parse(lexer);
                Precedence next;
                while ((next = nextOperator(lexer)) != null
                       && rightIsExpr(prec, next))
                    right = doShift(lexer, right, next.value);

                list.Add(right);
                return factory.make(list);
            }

        }
        public static String factoryName = "create";

        public abstract class Factory
        {
        public abstract ASTree make(Object arg);

        public static Factory getForASTList(System.Type clazz)
        {
            Factory f = get(clazz, typeof(List<ASTree>));
            if (f == null)
            {
                f = new FallBackFactory();
            }
            return f;
        }

        public static Factory get(System.Type clazz, System.Type argType)
            {
                Factory result = null;
                MethodInfo factoryFun = clazz.GetMethod(factoryName);

                if (factoryFun == null)
                {
                    Type[] types = new Type[1];
                    ConstructorInfo constructorFun = clazz.GetConstructor(types);

                    if (constructorFun != null)
                    {
                        ConstructorFactory _result = new ConstructorFactory();
                        _result.constructorFun = constructorFun;
                        result = _result;
                    }
                    else
                    {
                        result = new FallBackFactory();
                    }
                }
                else
                {
                    FunctionFactory _result = new FunctionFactory();
                    _result.factoryFun = factoryFun;
                    result = _result;
                }

                return result;
            }
        }


        public class ConstructorFactory : Factory
        {
            public ConstructorInfo constructorFun;

            public override ASTree make(Object arg)
            {
                return (ASTree)constructorFun.Invoke(new object[] { arg });
            }
        }

        public class FunctionFactory : Factory
        {
            public MethodInfo factoryFun;

            public override ASTree make(Object arg)
            {
                return (ASTree)factoryFun.Invoke(null, new object[] { arg });
            }
        }

        public class FallBackFactory : Factory
        {
            MethodInfo factoryFun;

            public override ASTree make(Object arg)
            {
                List<ASTree> argList = arg as List<ASTree>;
                if (argList.Count == 1)
                {
                    return argList[0];
                }
                else
                {
                    return new ASTList(argList);
                }
            }
        }



        protected List<Element> elements;

        Factory factory;
        public ASTree parse(Lexer lexer)
        {
            List<ASTree> results = new List<ASTree>();
            foreach (Element e in elements)
                e.parse(lexer, results);

            factory.make(results);
            return null;
        }
        protected bool match(Lexer lexer) 
        {
            if (elements.Count == 0)
                return true;
            else {
                Element e = elements[0];
                return e.match(lexer);
            }
        }
        protected Parser(Parser p)
        {
            elements = p.elements;
            factory = p.factory;
        }
        public Parser(System.Type clazz)
        {
            reset(clazz);
        }
        
        public static Parser rule() { return rule(null); }
        public static Parser rule(System.Type clazz)
        {
            return new Parser(clazz);
        }


        public Parser number(System.Type clazz)
        {
            elements.Add(new NumToken(clazz));
            return this;
        }
        public Parser number()
        {
            return number(null);
        }
        public Parser reset()
        {
            elements = new List<Element>();
            return this;
        }
        Parser reset(System.Type clazz)
        {
            factory = Factory.getForASTList(clazz);
            elements = new List<Element>();
            return this;
        }

        public Parser identifier(HashSet<String> reserved)
        {
            return identifier(null, reserved);
        }
        public Parser identifier(System.Type clazz,
                                 HashSet<String> reserved)
        {
            elements.Add(new IdToken(clazz, reserved));
            return this;
        }

        public Parser stringToken()
        {
            return stringToken(null);
        }
        public Parser stringToken(System.Type clazz)
        {
            elements.Add(new StrToken(clazz));
            return this;
        }
        public Parser token(String[] pat)
        {
            elements.Add(new Leaf(pat));
            return this;
        }
        public Parser sep(String[] pat)
        {
            elements.Add(new Skip(pat));
            return this;
        }
        public Parser ast(Parser p)
        {
            elements.Add(new Tree(p));
            return this;
        }
        public Parser or(Parser[] p)
        {
            elements.Add(new OrTree(p));
            return this;
        }

        public Parser maybe(Parser p)
        {
            Parser p2 = new Parser(p);
            p2.reset();
            elements.Add(new OrTree(new Parser[] { p, p2 }));
            return this;
        }
        public Parser option(Parser p)
        {
            elements.Add(new Repeat(p, true));
            return this;
        }
        public Parser repeat(Parser p)
        {
            elements.Add(new Repeat(p, false));
            return this;
        }
        public Parser expression(Parser subexp, Operators operators)
        {
            elements.Add(new Expr(null, subexp, operators));
            return this;
        }
        public Parser expression(System.Type clazz, Parser subexp,
                                 Operators operators)
        {
            elements.Add(new Expr(clazz, subexp, operators));
            return this;
        }
        public Parser insertChoice(Parser p)
        {
            Element e = elements[0];
            if (e is OrTree)
            {
                ((OrTree)e).insert(p);
            }
            else
            {
                Parser otherwise = new Parser(this);
                reset(null);
                or(new Parser[] { p, otherwise });
            }
            return this;
        }
    }


}
