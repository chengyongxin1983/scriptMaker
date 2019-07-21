using System;
namespace scriptMaker.vm
{
    public class Code
    {
        protected StoneVM svm;
        public int codeSize;
        protected int numOfStrings;
        public int nextReg;
        public int frameSize;

        public Code(StoneVM stoneVm)
        {
            svm = stoneVm;
            codeSize = 0;
            numOfStrings = 0;
        }
        public int position() { return codeSize; }
        public void set(short value, int pos)
        {
            svm.getCode()[pos] = (byte)(value >> 8);
            svm.getCode()[pos + 1] = (byte)value;
        }
        public void add(byte b)
        {
            svm.getCode()[codeSize++] = b;
        }
        public void add(Opcode.Code b)
        {
            add((byte)b);
        }

        public void add(short i)
        {
            add((byte)(i >> 8));
            add((byte)i);
        }
        public void add(int i)
        {
            add((byte)(i >> 24));
            add((byte)(i >> 16));
            add((byte)(i >> 8));
            add((byte)i);
        }
        public int record(String s)
        {
            svm.getStrings()[numOfStrings] = s;
            return numOfStrings++;
        }
    }
}
