using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class Name : ASTLeaf
    {
        public Name(Token t):base(t) { }
        public String name() { return token().getText(); }


        public override object eval(Environment env)
        {
            Object value = env.get(name());
            if (value == null)
                throw new ParseException("undefined name: " + name());
            else
                return value;
        }
    }
}
