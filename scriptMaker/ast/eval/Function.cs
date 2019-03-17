using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using static scriptMaker.parser.Parser;

namespace scriptMaker.ast
{
    public class Function {
        public ParameterList parameters;
        public BlockStmnt body;
        public Environment env;
        public Function(ParameterList _parameters, BlockStmnt _body, Environment _env) {
            parameters = _parameters;
            body = _body;
            env = _env;
        }
        public Environment makeEnv() { return new NestedEnv(env); }
        public override string ToString() { return "<fun:" + ">"; }

        public virtual object eval(Environment env)
        {
            return body.eval(env);
        }
    }

    public class NativeFunction : Function
    {
        string _navtiveClassName;
        static Assembly asa;

        public NativeFunction(ParameterList _parameters, Environment _env, string navtiveClassName):base(_parameters, null, _env)
        {
            _navtiveClassName = navtiveClassName;

        }
        public override string ToString() { return "<navtivefun:" + ">"; }

        public override object eval(Environment env)
        {
            object[] args = new object[parameters.size()];
            for (int i = 0; i < parameters.size(); ++i)
            {
                args[i] = env.get(parameters.name(i))
            }

            if (asa == null)
            {
                asa = Assembly.GetAssembly(typeof(NativeFunction));
            }
        }
    }
}