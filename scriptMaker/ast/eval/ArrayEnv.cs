using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using static scriptMaker.parser.Parser;

namespace scriptMaker.ast
{
    public class ArrayEnv : Environment
    {
        protected object[] values;
        protected ArrayEnv outer;

        public ArrayEnv()
        {
            Init(0, null);
        }

        public ArrayEnv(int size, Environment e)
        {
            Init(size, e);
        }

        void Init(int size, Environment e)
        {
            values = new object[size];
            outer = e;
        }

        public virtual Symbols symbols() { return null; }

        public void setOuter(Environment e) { outer = e; }

        public Object get(int nest, int index)
        {
            if (nest == 0)
            {
                return values[index];
            }
            else if (outer == null)
            {
                return null;
            }

            return outer.get(nest - 1, index);
        }
        public virtual void putNew(String name, Object value)
        {

        }
        public virtual void put(int nest, int index, object value)
        {
            if (nest == 0)
            {
                values[index] = value;
            }
            else if (outer == null)
            {
                return;
            }

            outer.put(nest - 1, index, value);
        }
        public virtual Environment where(String name)
        {
            return null;
        }

        public virtual void put(string name, object value)
        {
            // error here
        }

        public virtual object get(string name)
        {
            // error here

            return null;
        }

    }
}