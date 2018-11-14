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
        public BlockStmnt(List<ASTree> c):base(c) {  }
    }
}