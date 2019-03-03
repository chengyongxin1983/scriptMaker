using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class ClassInfo
    {
        protected ClassStmnt definition;
        protected Environment environment;
        protected ClassInfo superClass;

        public ClassInfo(ClassStmnt cs, Environment env)
        {
            definition = cs;
            environment = env;

            object obj = env.get(cs.superClass());

            if (obj == null)
            {
                superClass = null;
            }
            else if (obj is ClassInfo)
            {
                superClass = obj as ClassInfo;
            }
            else
            {
                throw new ParseException("error super class:" + cs.superClass());
            }
        }

        public string name() { return definition.name(); }

        public ClassInfo SuperClass() { return superClass; }

        public ClassBody body() { return definition.body(); }

        public Environment Environment() { return environment; }

        public override string ToString()
        {
            return "<class " + name() + ">";
        }

    }
}