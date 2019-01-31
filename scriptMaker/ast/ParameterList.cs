using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using static scriptMaker.parser.Parser;

namespace scriptMaker.ast
{

    public class ParameterList : ASTList
    {
        public ParameterList(List<ASTree> c):base(c) {  }
        public String name(int i) { return ((ASTLeaf)child(i)).token().getText(); }
        public int size() { return numChildren(); }
        public void eval(Environment env, int index, Object value)
        {
            env.putNew(name(index), value);
        }
    }
}