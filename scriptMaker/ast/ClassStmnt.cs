using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class ClassStmnt : ASTList
    {
        public ClassStmnt(List<ASTree> c) : base(c) { }

        public string name() { return ((ASTLeaf)child(0)).token().getText(); }

        public string superClass()
        {
            if (numChildren() < 3)
                return null;

            return ((ASTLeaf)child(1)).token().getText();
        }
        
        public ClassBody body()
        {
            return (ClassBody)child(numChildren() - 1);
        }

        public override string toString()
        {
            return "class " + name();
        }
    }
}