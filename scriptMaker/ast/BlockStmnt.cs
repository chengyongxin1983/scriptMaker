using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.parser
{

    public class BlockStmnt : ASTList
    {
        public BlockStmnt(List<ASTree> c) : base(c) { }

        public override Object eval(Environment env)
        {
            Object result = 0;
            foreach (ASTree t in _children)
            {
                if (!(t is NullStmnt))
                    result = ((ASTree)t).eval(env);
            }
            return result;
        }
    }
}