using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using scriptMaker.vm;

namespace scriptMaker.ast
{

    public class NumberLiteral : ASTLeaf
    {
        public NumberLiteral(Token t) : base(t) { }
        public int value() { return token().getNumber(); }

        public override object eval(Environment e) { return value(); }

        public override void compile(Code c)
        {
            int v = value();
            if (Byte.MinValue <= v && v <= Byte.MaxValue)
            {
                c.add(Opcode.Code.BCONST);
                c.add((byte)v);
            }
            else
            {
                c.add(Opcode.Code.ICONST);
                c.add(v);
            }
            c.add(Opcode.encodeRegister(c.nextReg++));
        }


    }
    
}