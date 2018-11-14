using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{
    public class WhileStmnt : ASTList
    {
        public WhileStmnt(List<ASTree> c) : base(c) { }
        public ASTree condition() { return child(0); }
        public ASTree body() { return child(1); }
        public String toString()
        {
            return "(while " + condition() + " " + body() + ")";
        }
    }
}