using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using static scriptMaker.parser.Parser;

namespace scriptMaker.ast
{
    public abstract class Postfix : ASTList
    {
        public Postfix(List<ASTree> c):base(c) { }

        public abstract object eval(Environment env, object value);
    }
}