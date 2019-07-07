using System;
namespace scriptMaker.vm
{

    public interface HeapMemory
    {
        object read(int index);
        void write(int index, object v);
    }
}
