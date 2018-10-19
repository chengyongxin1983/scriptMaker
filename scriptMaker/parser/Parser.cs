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
            protected Tree(Parser p) { parser = p; }
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
            protected OrTree(Parser[] p) { parsers = p; }
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
            protected void insert(Parser p)
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
            protected Repeat(Parser p, bool once) { parser = p; onlyOnce = once; }
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
        

        
        public static String factoryName = "create";

        public abstract class Factory
        {
            public abstract ASTree make(List<ASTree> arg);

            public static Factory get(System.Type clazz)
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

            public override ASTree make(List<ASTree> arg)
            {
                return (ASTree)constructorFun.Invoke(new object[] { arg });
            }
        }

        public class FunctionFactory : Factory
        {
            public MethodInfo factoryFun;

            public override ASTree make(List<ASTree> arg)
            {
                return (ASTree)factoryFun.Invoke(null, new object[] { arg });
            }
        }

        public class FallBackFactory : Factory
        {
            MethodInfo factoryFun;

            public override ASTree make(List<ASTree> arg)
            {
                if (arg.Count == 1)
                {
                    return arg[0];
                }
                else
                {
                    return new ASTList(arg);
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
           
            make(results);
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

        public Parser(System.Type clazz)
        {
            reset(clazz);
        }

        
        ASTree make(List<ASTree> arg)
        {
           


            return null;
        }
        Parser reset(System.Type clazz)
        {
            factory = Factory.get(clazz);

            return this;
        }
    }


}
