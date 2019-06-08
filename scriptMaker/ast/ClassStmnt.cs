using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class ClassStmnt : ASTList
    {
        public ClassStmnt(List<ASTree> c) : base(c) { }

        public string name() { return ((ASTLeaf)child(0)).token().getText(); }

        public string superClass()
        {
            if (numChildren() < 3)
                return null;

            return ((ASTLeaf)child(1)).token().getText();
        }
        
        public ClassBody body()
        {
            return (ClassBody)child(numChildren() - 1);
        }

        public override string toString()
        {
            return "class " + name();
        }

        public override object eval(Environment env)
        {
            //ClassInfo ci = new ClassInfo(this, env);
            //env.put(name(), ci);
            //return name();

            Symbols methodNames = new MemberSymbols(env.symbols(),
                                                   MemberSymbols.METHOD);
            Symbols fieldNames = new MemberSymbols(methodNames,
                                                   MemberSymbols.FIELD);
            ClassInfo ci = new ClassInfo(this, env, methodNames,
                                               fieldNames);
            env.put(name(), ci);
            List<DefStmnt> methods = new List<DefStmnt>();
            if (ci.SuperClass() != null)
                ci.SuperClass().copyTo(fieldNames, methodNames, methods);
            Symbols newSyms = new SymbolThis(fieldNames);
            body().lookup(newSyms, methodNames, fieldNames,
                                         methods);
            ci.setMethods(methods);
            return name();
        }
    }
}