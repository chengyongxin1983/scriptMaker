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

        protected int size;
        public Function(ParameterList _parameters, BlockStmnt _body, Environment _env, int memorysize) {
            parameters = _parameters;
            body = _body;
            env = _env;
            size = memorysize;
        }
        public virtual Environment makeEnv()
        {
            var e = new ArrayEnv(size, env);
            return e;
        }



        public override string ToString() { return "<fun:" + ">"; }

        public virtual object eval(Environment env)
        {
            return body.eval(env);
        }
    }

    public class NativeFunction 
    {
        static Assembly ass;
        MethodInfo method;

        public NativeFunction(string nativeClassName, string nativeMethodName, Type[] types)
        {
            if (ass == null)
            {
                ass = Assembly.Load("mscorlib");
            }

            Type type = ass.GetType(nativeClassName);
            method = type.GetMethod(nativeMethodName, types);
        }
        public override string ToString() { return "<navtivefun:" + ">"; }

        public object eval(object[] args)
        {
            object result = null;
            if (method != null)
            {
                result = method.Invoke(null, args);
            }

            return result;
        }

        public static void AddNativeFunctions(Environment env)
        {
            Type[] types = new Type[1] { typeof(string), };
            Symbols.Location loc = env.symbols().put("print");
            env.put(loc.nest, loc.index, new NativeFunction("System.Console", "Write", types));
        }
    }




    public class OptMethod : Function
    {
        OptStoneObject self;
         public OptMethod(ParameterList parameters, BlockStmnt body,
                     Environment env, int memorySize, OptStoneObject _self):
        base(parameters, body, env, memorySize)
        {
            self = _self;
        }
        public override Environment makeEnv()
        {
            ArrayEnv e = new ArrayEnv(size, env);
            e.put(0, 0, self);
            return e;
        }
    }

}