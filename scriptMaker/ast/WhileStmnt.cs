using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using scriptMaker.vm;

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

        public override void compile(Code c)
        {
            int oldReg = c.nextReg;
            c.add(Opcode.Code.BCONST);
            c.add((byte)0);
            c.add(Opcode.encodeRegister(c.nextReg++));
            int pos = c.position();
            condition().compile(c);
            int pos2 = c.position();
            c.add(Opcode.Code.IFZERO);
            c.add(Opcode.encodeRegister(--c.nextReg));
            c.add(Opcode.encodeShortOffset(0));
            c.nextReg = oldReg;
            body().compile(c);
            int pos3 = c.position();
            c.add(Opcode.Code.GOTO);
            c.add(Opcode.encodeShortOffset(pos - pos3));
            c.set(Opcode.encodeShortOffset(c.position() - pos2), pos2 + 2);
        }
    }
}