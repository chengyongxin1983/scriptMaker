using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using scriptMaker.vm;

namespace scriptMaker.ast
{ 

    public class StringLiteral : ASTLeaf
    {
        public StringLiteral(Token t):base(t) { }
        public String value() { return token().getText(); }

        public override object eval(Environment e) { return value(); }

        public override void compile(Code c)
        {
            int i = c.record(value());
            c.add(Opcode.Code.SCONST);
            c.add(Opcode.encodeShortOffset(i));
            c.add(Opcode.encodeRegister(c.nextReg++));
        }

    }
}