using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{
    public class NullStmnt : ASTList {
        public NullStmnt(List<ASTree> c):base(c) {  }
    }
}