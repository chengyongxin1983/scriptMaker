using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.parser
{ 

    public class StringLiteral : ASTLeaf
    {
        public StringLiteral(Token t):base(t) { }
        public String value() { return token().getText(); }

        public override object eval(Environment e) { return value(); }
    }
}