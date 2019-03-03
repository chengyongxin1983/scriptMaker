using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{
    public class WhileStmnt : ASTList
    {
        public WhileStmnt(List<ASTree> c) : base(c) { }
        public ASTree condition() { return child(0); }
        public ASTree body() { return child(1); }
        public override string toString()
        {
            return "(while " + condition() + " " + body() + ")";
        }

        public override Object eval(Environment env)
        {
            Object result = 0;
            for (;;)
            {
                Object c = ((ASTree)condition()).eval(env);
                if (c is System.Int32 && ((System.Int32)c) == 0)
                    return result;
                else
                    result = ((ASTree)body()).eval(env);
        }
    }
}
}