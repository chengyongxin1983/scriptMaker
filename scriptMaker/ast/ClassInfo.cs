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

        protected Symbols methods, fields;
        protected DefStmnt[] methodDefs;

        public int size() { return fields.size(); }

        public void copyTo(Symbols f, Symbols m, List<DefStmnt> mlist)
        {
            f.append(fields);
            m.append(methods);
            foreach (DefStmnt def in methodDefs)
                mlist.Add(def);
        }

        public int fieldIndex(String name, out bool bFind) 
        {
            return fields.find(name, out bFind); 

        }

        public int methodIndex(String name, out bool bFind) 
        {
             return methods.find(name, out bFind);
        }


        public void setMethods(List<DefStmnt> methods)
        {
            methodDefs = methods.ToArray();
        }

        protected ClassStmnt definition;

        protected Environment environment;
        protected ClassInfo superClass;


    public ClassInfo(ClassStmnt cs, Environment env, Symbols _methods,
                        Symbols _fields)
    {
            definition = cs;
            environment = env;
            methods = _methods;
            fields = _fields;
            methodDefs = null;
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

        public Object method(OptStoneObject self, int index)
        {
            DefStmnt def = methodDefs[index];
            return new OptMethod(def.parameters(), def.body(), environment,
                                 (def).locals(), self);
        }


    }
}