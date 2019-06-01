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
        protected int index;
        protected int size;

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
            ((Environment)env).put(0, index, new Function(parameters(), body(), env, size));
            return name();
        }

        public override void lookup(Symbols syms)
        {
            index = syms.putNew(name());
            size = Fun.lookup(syms, parameters(), body());
        }
    }
}