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
            if (value is StoneObject)
            {
                return ((StoneObject)value).Read(name());
            }
            else if (value is ClassInfo)
            {
                if (name() == "new")
                {
                    ClassInfo ci = value as ClassInfo;
                    Environment newenv = new ArrayEnv(10, ci.Environment());
                    StoneObject so = new StoneObject(newenv);
                    newenv.put("this", so);
                    InitObject(ci, env);
                    return so;
                }

            }
            return null;
        }

        protected void InitObject(ClassInfo ci, Environment env)
        {
            if (ci.SuperClass() != null)
            {
                InitObject(ci.SuperClass(), env);
            }

            ci.body().eval(env);
        }

        public override string toString()
        {
            return ". " + name();
        }
    }
}