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
    public class NestedEnv : Environment
    {
        protected Dictionary<string, object> values;
        protected Environment outer;
        public NestedEnv() { Init(null); }
        public NestedEnv(Environment e)
        {
            Init(e);
        }

        void Init(Environment e)
        {
            values = new Dictionary<string, object>();
            outer = e;
        }

        public void setOuter(Environment e) { outer = e; }
        public Object get(String name)
        {
            object v = null;
            if (!string.IsNullOrEmpty(name))
            {
                values.TryGetValue(name, out v);
            }

            if (v == null && outer != null)
            {

                return outer.get(name);
            }
            return v;
        }
        public void putNew(String name, Object value)
        {
            if (values.ContainsKey(name))
            {
                values[name] = value;
            }
            else
            {
                values.Add(name, value);
            }
        }
        public void put(String name, Object value)
        {
            Environment e = where(name);
            if (e == null)
                e = this;
            e.putNew(name, value);
        }
        public Environment where(String name)
        {
            if (values.ContainsKey(name))
                return this;
            else if (outer == null)
                return null;
            else
                return ((Environment)outer).where(name);
        }
    }
}