using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class StoneObject
    {
        public Environment env;
        public StoneObject(Environment e)
        {
            env = e;
        }

        public void Write(string name, object value)
        {
            Environment e = getEnv(name);
            if (e != null)
            {
                e.put(name, value);
            }
        }

        public object Read(string name)
        {
            object result = null;
           Environment e = getEnv(name);
            if (e != null)
            {
                result = e.get(name);
            }

            return result;
        }

        protected Environment getEnv(string name)
        {
            Environment e = env.where(name);
            if (e != null && e == env)
            {
                return e;
            }

            throw new ParseException("env");
        }
        public string toString()
        {
            return "<object " + GetHashCode() + ">";
        }

    }
}