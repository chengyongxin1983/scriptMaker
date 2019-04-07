
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{
    public class ArrayRef : Postfix
    {
        public ArrayRef(List<ASTree> list) : base(list)
        {

        }

        public ASTree index()
        {
            return child(0);
        }

        public override string ToString()
        {
            return "[" + index() + "]";
        }


        public override object eval(Environment env, object value)
        {
            if (value is object[] )
            {
                object indexObj = index().eval(env);
                if (indexObj is System.Int32)
                {
                    return ((object[])value)[(System.Int32)indexObj];
                }
            }

            return null;
        }
    }
}