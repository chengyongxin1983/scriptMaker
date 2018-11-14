using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.parser
{

    public class Name : ASTLeaf
    {
        public Name(Token t):base(t) { }
        public String name() { return token().getText(); }
    }
}
