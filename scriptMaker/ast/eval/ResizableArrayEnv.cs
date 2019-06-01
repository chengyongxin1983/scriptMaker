using System;
namespace scriptMaker.ast
{
    public class ResizableArrayEnv : ArrayEnv
    {
        protected Symbols names;


        public ResizableArrayEnv() : base(10, null)
        {
            names = new Symbols();
        }

        public override Symbols symbols() { return names; }

        public override object get(string name)
        {
            bool bFind = false;
            int result = names.find(name, out bFind);
            if (bFind)
            {
                return values[result];
            }

            if (outer == null)
            {
                return null;
            }

            return outer.get(name);
        }

        public override void put(string name, object value)
        {
            Environment e = where(name);
            if (e == null)
            {
                e = this;
            }

            e.putNew(name, value);
        }


        protected void assign(int index, object value)
        {
            if (index >= values.Length)
            {
                int newLen = values.Length * 2;
                if (index >= newLen)
                {
                    newLen = index + 1;
                }

                object[] newValues = new object[newLen];

                Array.Copy(values, newValues, values.Length);
                values = newValues;
            }

            values[index] = value;
        }

        public override void putNew(string name, object value)
        {
            assign(names.putNew(name), value);
        }


        public override Environment where(String name)
        {
            bool bFind = false;
            int result = names.find(name, out bFind);
            if (bFind)
            {
                return this;
            }

            if (outer == null)
            {
                return null;
            }

            return outer.where(name);
        }

        public override void put(int nest, int index, object value)
        {
            if (nest == 0)
            {
                assign(index, value);
            }
            else 
            {
                base.put(nest - 1, index, value);
            }

        }

    }
}
