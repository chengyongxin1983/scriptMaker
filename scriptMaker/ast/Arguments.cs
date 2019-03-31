using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using static scriptMaker.parser.Parser;

namespace scriptMaker.ast
{

    public class Arguments : Postfix
    {
        public Arguments(List<ASTree> c):base(c) {  }
        public int size() { return numChildren(); }
        
        public override object eval(Environment callerEnv, object value)
        {
            if (value is Function)
            {
                Function func = (Function)value;
                ParameterList parameters = func.parameters;
                if (size() != parameters.size())
                    throw new ParseException("bad number of arguments");
                Environment newEnv = func.makeEnv();
                int num = 0;

                for (int i = 0; i < numChildren(); ++i)
                {
                    ASTree a = child(i);

                    parameters.eval(newEnv, num++, a.eval(callerEnv));
                }
                return func.eval(newEnv);
            }
            else if (value is NativeFunction)
            {
                NativeFunction func = (NativeFunction)value;
                int num = 0;

                object[] args = new object[numChildren()];
                for (int i = 0; i < numChildren(); ++i)
                {
                    ASTree a = child(i);

                    args[i] = a.eval(callerEnv);
                }
                return func.eval(args);
            }
            else
            {
                throw new ParseException("bad function");
            }

        }
    }
}