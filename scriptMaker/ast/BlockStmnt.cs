using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using scriptMaker.vm;

namespace scriptMaker.ast
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
        public override void compile(Code c)
        {
            if (this.numChildren() > 0)
            {
                int initReg = c.nextReg;
                foreach (ASTree t in _children)
                {
                    c.nextReg = initReg;
                    t.compile(c);
                }
            }
            else
            {
                c.add(Opcode.Code.BCONST);
                c.add((byte)0);
                c.add(Opcode.encodeRegister(c.nextReg++));
            }
        }
    }
}