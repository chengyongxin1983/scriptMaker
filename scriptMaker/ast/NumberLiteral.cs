using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class NumberLiteral : ASTLeaf
    {
        public NumberLiteral(Token t) : base(t) { }
        public int value() { return token().getNumber(); }

        public override object eval(Environment e) { return value(); }
    }
    
}