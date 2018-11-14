using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{
    public class IfStmnt : ASTList
    {
        public IfStmnt(List<ASTree> c):base(c) { }
        public ASTree condition() { return child(0); }
        public ASTree thenBlock() { return child(1); }
        public ASTree elseBlock() {
            return numChildren() > 2 ? child(2) : null;
        }
        public String toString() {
            return "(if " + condition() + " " + thenBlock()
                 + " else " + elseBlock() + ")";
        }
    }
}