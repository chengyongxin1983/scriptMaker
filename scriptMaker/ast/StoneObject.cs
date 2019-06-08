using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class OptStoneObject
    {
        public ClassInfo classInfo;
        protected object[] fields;

        public OptStoneObject(ClassInfo ci, int size)
        {
            classInfo = ci;
            fields = new object[size];
        }

        public object Read(string name)
        {
            bool bFind = false;
            int i = classInfo.fieldIndex(name, out bFind);
            if (bFind)
            {
                return fields[i];
            }
            else
            {
                bFind = false;
                int im = classInfo.methodIndex(name, out bFind);
                if (bFind)
                {
                    return method(im);
                }
            }

            throw new Exception();
        }

        public object method(int index)
        {
            return classInfo.method(this, index);
        }


        public void write(string name, object value)
        {
            bool bFind = false;
            int i = classInfo.fieldIndex(name, out bFind);
            if (!bFind)
            {

            }
            else
            {
                fields[i] = value;
            }
        }

        public object read(int index)
        {
            return fields[index];
        }

        public void write(int index, object value)
        {
            fields[index] = value;
        }
    }

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