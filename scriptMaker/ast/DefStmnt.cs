using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using static scriptMaker.parser.Parser;

namespace scriptMaker.ast
{

    public class DefStmnt : ASTList
    {
        public DefStmnt(List<ASTree> c):base(c) {  }
        public String name() { return ((ASTLeaf)child(0)).token().getText(); }
        public ParameterList parameters() { return (ParameterList)child(1); }
        public BlockStmnt body() { return (BlockStmnt)child(2); }
        public override String ToString()
        {
            return "(def " + name() + " " + parameters() + " " + body() + ")";
        }

        public override Object eval(Environment env)
        {
            ((Environment)env).putNew(name(), new Function(parameters(), body(), env));
            return name();
        }
    }
}