using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using scriptMaker.vm;

namespace scriptMaker.ast
{
    public abstract class ASTree
    {
        public abstract ASTree child(int i);
        public abstract int numChildren();
        public abstract String location();

        public abstract IEnumerator<ASTree> children();
        public abstract Object eval(Environment env);
        public abstract void lookup(Symbols syms);
        public abstract void compile(Code c);

    }

}
