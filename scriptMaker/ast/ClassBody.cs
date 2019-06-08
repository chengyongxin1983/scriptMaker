using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class ClassBody : ASTList
    {
        public ClassBody(List<ASTree> c) : base(c) { }

        public override object eval(Environment env)
        {
            Object result = 0;
            foreach (ASTree t in _children)
            {
                if (!(t is DefStmnt))
                    result = ((ASTree)t).eval(env);
            }
            return null;
        }

        public void lookup(Symbols syms, Symbols methodNames,
                          Symbols fieldNames, List<DefStmnt> methods)
        {
            foreach (ASTree t in _children)
            {
                if (t is DefStmnt) 
                {
                    DefStmnt def = (DefStmnt)t;
                    int oldSize = methodNames.size();
                    int i = methodNames.putNew(def.name());
                    if (i >= oldSize)
                        methods.Add(def);
                    else
                        methods[i] = def;
                    ((DefStmnt)def).lookupAsMethod(fieldNames);
                }
                else
                    t.lookup(syms);
        }
    }
}
}