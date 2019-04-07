
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{
    public class Arrayliteral : ASTList
    {
        public Arrayliteral(List<ASTree> list) : base(list)
        {

        }

        public int size()
        {
            return numChildren();
        }

        public override object eval(Environment env)
        {
            int s = numChildren();
            object[] res = new object[s];
            int i = 0;
            foreach (ASTree t in _children)
            {
                res[i++] = t.eval(env);
            }

            return res;
        }
    }
}