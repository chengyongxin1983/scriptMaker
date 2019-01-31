using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{
    public class NegativeExpr : ASTList
    {
        public NegativeExpr(List<ASTree> c):base(c) { }
        public ASTree operand() { return child(0); }
        public String toString() {
            return "-" + operand();
        }
    }
}