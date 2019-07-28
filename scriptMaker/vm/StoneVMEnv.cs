using System;
using scriptMaker.ast;

namespace scriptMaker.vm
{
    public class StoneVMEnv : ResizableArrayEnv, HeapMemory
    {
        protected StoneVM svm;
        protected Code _code;
        public StoneVMEnv(int codeSize, int stackSize, int stringsSize)
        {
            svm = new StoneVM(codeSize, stackSize, stringsSize, this);
            _code = new Code(svm);
        }
        public override StoneVM stoneVM() { return svm; }
        public override Code code() { return _code; }
        public Object read(int index) { return values[index]; }
        public void write(int index, Object v) { values[index] = v; }
    }
}
