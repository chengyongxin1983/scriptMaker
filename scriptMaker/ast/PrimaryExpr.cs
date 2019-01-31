using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{ 
    public class PrimaryExpr : ASTList {
        public PrimaryExpr(List<ASTree> c):base(c) { }
        public static ASTree create(List<ASTree> c) {
            return c.Count() == 1 ? c[0] : new PrimaryExpr(c);
        }
        
        public ASTree operand() { return child(0); }
        public Postfix postfix(int nest)
        {
            return (Postfix)child(numChildren() - nest - 1);
        }
        public bool hasPostfix(int nest) { return numChildren() - nest > 1; }
        public override object eval(Environment env)
        {
            return evalSubExpr(env, 0);
        }
        public Object evalSubExpr(Environment env, int nest)
        {
            if (hasPostfix(nest))
            {
                Object target = evalSubExpr(env, nest + 1);
                return ((Postfix)postfix(nest)).eval(env, target);
            }
            else
                return ((ASTree)operand()).eval(env);
        }
    }
}
