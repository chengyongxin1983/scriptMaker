using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{
    public class IfStmnt : ASTList
    {
        public IfStmnt(List<ASTree> c):base(c) { }
        public ASTree condition() { return child(0); }
        public ASTree thenBlock() { return child(1); }
        public ASTree elseBlock() {
            return numChildren() > 2 ? child(2) : null;
        }
        public override string toString() {
            return "(if " + condition() + " " + thenBlock()
                 + " else " + elseBlock() + ")";
        }

        public override Object eval(Environment env)
        {
            Object c = ((ASTree)condition()).eval(env);
            if ((c is System.Int32 && ((System.Int32)c) != 0 )
                ||(c is Boolean && (Boolean)c))
                return ((ASTree)thenBlock()).eval(env);
            else {
                ASTree b = elseBlock();
                if (b == null)
                    return 0;
                else
                    return ((ASTree)b).eval(env);
            }
        }
    }
}