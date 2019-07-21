using System;
using scriptMaker.ast;

namespace scriptMaker.vm
{
    public class VmFunction : Function
    {
        protected int entry;
        public VmFunction(ParameterList parameters, BlockStmnt body,
                          Environment env, int _entry)
            :base(parameters, body, env, 1000)
        {
            this.entry = _entry;
        }
        public int getEntry() { return entry; }
    }
}
