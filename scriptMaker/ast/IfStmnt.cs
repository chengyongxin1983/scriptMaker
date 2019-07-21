using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using scriptMaker.vm;

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

        public override void compile(Code c)
        {
            condition().compile(c);
            int pos = c.position();
            c.add(Opcode.Code.IFZERO);
            c.add(Opcode.encodeRegister(--c.nextReg));
            c.add(Opcode.encodeShortOffset(0));
            int oldReg = c.nextReg;
            thenBlock().compile(c);
            int pos2 = c.position();
            c.add(Opcode.Code.GOTO);
            c.add(Opcode.encodeShortOffset(0));
            c.set(Opcode.encodeShortOffset(c.position() - pos), pos + 2);
            ASTree b = elseBlock();
            c.nextReg = oldReg;
            if (b != null)
                b.compile(c);
            else
            {
                c.add(Opcode.Code.BCONST);
                c.add((byte)0);
                c.add(Opcode.encodeRegister(c.nextReg++));
            }
            c.set(Opcode.encodeShortOffset(c.position() - pos2), pos2 + 1);
        }
    }
}