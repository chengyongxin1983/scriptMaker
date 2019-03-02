using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class ClassBody : ASTList
    {
        public ClassBody(List<ASTree> c) : base(c) { }
    }
}