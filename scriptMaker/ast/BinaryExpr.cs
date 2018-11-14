using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.parser
{
    public class BinaryExpr : ASTList
    {
        public BinaryExpr(List<ASTree> c):base(c) {  }
        public ASTree left() { return child(0); }

        public ASTree right() { return child(2); }
    }
}