using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using scriptMaker.vm;
using static scriptMaker.parser.Parser;

namespace scriptMaker.ast
{

    public class Arguments : Postfix
    {
        public Arguments(List<ASTree> c):base(c) {  }
        public int size() { return numChildren(); }
        
        public override object eval(Environment callerEnv, object value)
        {
            if (value is VmFunction)
            {
                VmFunction func = (VmFunction)value;
                ParameterList parameters = func.parameters;
                if (size() != parameters.size())
                    throw new ParseException("bad number of arguments");
                int num = 0;

                for (int i = 0; i < numChildren(); ++i)
                {
                    ASTree a = child(i);

                    parameters.eval(callerEnv, num++, a.eval(callerEnv));
                }

                StoneVM svm = callerEnv.stoneVM();
                svm.run(func.getEntry());
                return svm.getStack()[0];
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
            else if (value is Function)
            {
                Function func = (Function)value;
                ParameterList parameters = func.parameters;
                if (size() != parameters.size())
                    throw new ParseException("bad number of arguments");
                int num = 0;

                for (int i = 0; i < numChildren(); ++i)
                {
                    ASTree a = child(i);

                    parameters.eval(callerEnv, num++, a.eval(callerEnv));
                }

                Environment newEnv = func.makeEnv();
                return func.body.eval(newEnv);
            }
            else 
            {
                throw new ParseException("bad function");
            }

        }

        public override void compile(Code c)
        {
            int newOffset = c.frameSize;
            int numOfArgs = 0;
            for (int i = 0; i < numChildren(); ++i)
            {
                ASTree a = child(i);

                a.compile(c);
                c.add(Opcode.Code.MOVE);
                c.add(Opcode.encodeRegister(--c.nextReg));
                c.add(Opcode.encodeOffset(newOffset++));
                numOfArgs++;
            }
            c.add(Opcode.Code.CALL);
            c.add(Opcode.encodeRegister(--c.nextReg));
            c.add(Opcode.encodeOffset(numOfArgs));
            c.add(Opcode.Code.MOVE);
            c.add(Opcode.encodeOffset(c.frameSize));
            c.add(Opcode.encodeRegister(c.nextReg++));
        }
    }
}