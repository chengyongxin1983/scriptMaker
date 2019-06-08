using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class Dot : Postfix
    {
        public Dot(List<ASTree> c) : base(c) { }

        public string name() { return ((ASTLeaf)child(0)).token().getText(); }


        public override object eval(Environment env, object value)
        {
            if (value is OptStoneObject)
            {
                return ((OptStoneObject)value).Read(name());
            }
            else if (value is ClassInfo)
            {
                if (name() == "new")
                {
                    ClassInfo ci = (ClassInfo)value;
                    ArrayEnv newEnv = new ArrayEnv(1, ci.Environment());
                    OptStoneObject so = new OptStoneObject(ci, ci.size());
                    newEnv.put(0, 0, so);
                    initObject(ci, so, newEnv);
                    return so;
                }

            }
            return null;
        }

        protected void initObject(ClassInfo ci, OptStoneObject obj,
                                 Environment env)
        {
            if (ci.SuperClass() != null)
                initObject(ci.SuperClass(), obj, env);
            (ci.body()).eval(env);
        }

        protected void InitObject(ClassInfo ci, OptStoneObject obj, Environment env)
        {
            if (ci.SuperClass() != null)
                initObject(ci.SuperClass(), obj, env);
            (ci.body()).eval(env);
        }

        public override string toString()
        {
            return ". " + name();
        }
    }
}