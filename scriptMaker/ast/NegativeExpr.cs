using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using scriptMaker.vm;

namespace scriptMaker.ast
{
    public class NegativeExpr : ASTList
    {
        public NegativeExpr(List<ASTree> c):base(c) { }
        public ASTree operand() { return child(0); }
        public override String toString() {
            return "-" + operand();
        }

        public override void compile(Code c)
        {
            (operand()).compile(c);
            c.add(Opcode.Code.NEG);
            c.add(Opcode.encodeRegister(c.nextReg - 1));
        }
    }
}