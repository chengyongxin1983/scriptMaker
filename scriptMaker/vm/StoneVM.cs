using System;
namespace scriptMaker.vm
{
    public class StoneVM
    {
        protected byte[] code;
        protected object[] stack;
        protected string[] strings;
        protected HeapMemory heap;

        public int pc, fp, sp, ret;

        protected object[] registers;

        public static int NUM_OF_REG = 6;
        public static int SAVE_AREA_SIZE = NUM_OF_REG + 2;

        public static int TRUE = 1;
        public static int FALSE = 0;

        public StoneVM(int codeSize, int stackSize, int stringSize, HeapMemory hm)
        {
            code = new byte[codeSize];
            stack = new object[stackSize];
            strings = new string[stringSize];
            registers = new object[NUM_OF_REG];
            heap = hm;
        }

        public object getReg(int i) { return registers[i]; }
        public void setReg(int i, object value) { registers[i] = value; }
        public String[] getStrings() { return strings; }
        public byte[] getCode() { return code; }
        public object[] getStack() { return stack; }
        public HeapMemory getHeap() { return heap; }


        protected void run(int entry)
        {
            pc = entry;
            fp = 0;
            sp = 0;
            ret = -1;
            while (pc >= 0)
            {
                mainLoop();
            }
        }



        public static int readInt(byte[] array, int index)
        {
            return (array[index] << 24) | ((array[index + 1] & 0xff) << 16)
                   | ((array[index + 2] & 0xff) << 8) | (array[index + 3] & 0xff);
        }
        public static int readShort(byte[] array, int index)
        {
            return (array[index] << 8) | (array[index + 1] & 0xff);
        }

        protected void mainLoop()
        {
            switch ((Opcode.Code)code[pc])
            {
                case Opcode.Code.ICONST:
                    registers[Opcode.decodeRegister(code[pc + 5])] = readInt(code, pc + 1);
                    pc += 6;
                    break;
                case Opcode.Code.BCONST:
                    registers[Opcode.decodeRegister(code[pc + 2])] = (int)code[pc + 1];
                    pc += 3;
                    break;
                case Opcode.Code.SCONST:
                    registers[Opcode.decodeRegister(code[pc + 3])]
                        = strings[readShort(code, pc + 1)];
                    pc += 4;
                    break;
                case Opcode.Code.MOVE:
                    moveValue();
                    break;
                case Opcode.Code.GMOVE:
                    moveHeapValue();
                    break;
                case Opcode.Code.IFZERO:
                    {
                        Object value = registers[Opcode.decodeRegister(code[pc + 1])];
                        if ((value is System.Int32 || value is System.UInt32)
                         && ((UInt32)value) == 0)
                        {
                            pc += readShort(code, pc + 2);
                        }
                        else
                            pc += 4;
                        break;
                    }
                case Opcode.Code.GOTO:
                    pc += readShort(code, pc + 1);
                    break;
                case Opcode.Code.CALL:
                    callFunction();
                    break;
                case Opcode.Code.RETURN:
                    pc = ret;
                    break;
                case Opcode.Code.SAVE:
                    saveRegisters();
                    break;
                case Opcode.Code.RESTORE:
                    restoreRegisters();
                    break;
                case Opcode.Code.NEG:
                    {
                        int reg = Opcode.decodeRegister(code[pc + 1]);
                        Object v = registers[reg];
                        if (v is System.Int32)
                        {
                            registers[reg] = -((int)v);
                        }
                        else
                            throw new Exception("bad operand value");
                        pc += 2;
                        break;
                    }
                default:
                    if (code[pc] > LESS)
                        throw new Exception("bad instruction");
                    else
                        computeNumber();
                    break;
            }
        }


        protected void moveValue()
        {
            byte src = code[pc + 1];
            byte dest = code[pc + 2];
            Object value;
            if (Opcode.isRegister(src))
                value = registers[Opcode.decodeRegister(src)];
            else
                value = stack[fp + Opcode.decodeOffset(src)];
            if (Opcode.isRegister(dest))
                registers[Opcode.decodeRegister(dest)] = value;
            else
                stack[fp + Opcode.decodeOffset(dest)] = value;
            pc += 3;
        }
        protected void moveHeapValue()
        {
            byte rand = code[pc + 1];
            if (Opcode.isRegister(rand))
            {
                int dest = readShort(code, pc + 2);
                heap.write(dest, registers[Opcode.decodeRegister(rand)]);
            }
            else
            {
                int src = readShort(code, pc + 1);
                registers[Opcode.decodeRegister(code[pc + 3])] = heap.read(src);
            }
            pc += 4;
        }
        protected void callFunction()
        {
            Object value = registers[Opcode.decodeRegister(code[pc + 1])];
            int numOfArgs = code[pc + 2];
            if (value instanceof VmFunction
            && ((VmFunction)value).parameters().size() == numOfArgs) {
                ret = pc + 3;
                pc = ((VmFunction)value).entry();
            }
        else if (value instanceof NativeFunction
                && ((NativeFunction)value).numOfParameters() == numOfArgs) {
                Object[] args = new Object[numOfArgs];
                for (int i = 0; i < numOfArgs; i++)
                    args[i] = stack[sp + i];
                stack[sp] = ((NativeFunction)value).invoke(args,
                                            new ASTList(new ArrayList<ASTree>()));
                pc += 3;
            }
        else
            throw new Exception("bad function call");
        }
        protected void saveRegisters()
        {
            int size = Opcode.decodeOffset(code[pc + 1]);
            int dest = size + sp;
            for (int i = 0; i < NUM_OF_REG; i++)
                stack[dest++] = registers[i];
            stack[dest++] = fp;
            fp = sp;
            sp += size + SAVE_AREA_SIZE;
            stack[dest++] = ret;
            pc += 2;
        }
        protected void restoreRegisters()
        {
            int dest = Opcode.decodeOffset(code[pc + 1]) + fp;
            for (int i = 0; i < NUM_OF_REG; i++)
                registers[i] = stack[dest++];
            sp = fp;
            fp = ((Integer)stack[dest++]).intValue();
            ret = ((Integer)stack[dest++]).intValue();
            pc += 2;
        }
        protected void computeNumber()
        {
            int left = Opcode.decodeRegister(code[pc + 1]);
            int right = Opcode.decodeRegister(code[pc + 2]);
            Object v1 = registers[left];
            Object v2 = registers[right];
            boolean areNumbers = v1 instanceof Integer && v2 instanceof Integer;
            if (code[pc] == ADD && !areNumbers)
                registers[left] = String.valueOf(v1) + String.valueOf(v2);
            else if (code[pc] == EQUAL && !areNumbers)
            {
                if (v1 == null)
                    registers[left] = v2 == null ? TRUE : FALSE;
                else
                    registers[left] = v1.equals(v2) ? TRUE : FALSE;
            }
            else
            {
                if (!areNumbers)
                    throw new StoneException("bad operand value");
                int i1 = ((Integer)v1).intValue();
                int i2 = ((Integer)v2).intValue();
                int i3;
                switch (code[pc])
                {
                    case ADD:
                        i3 = i1 + i2;
                        break;
                    case SUB:
                        i3 = i1 - i2;
                        break;
                    case MUL:
                        i3 = i1 * i2;
                        break;
                    case DIV:
                        i3 = i1 / i2;
                        break;
                    case REM:
                        i3 = i1 % i2;
                        break;
                    case EQUAL:
                        i3 = i1 == i2 ? TRUE : FALSE;
                        break;
                    case MORE:
                        i3 = i1 > i2 ? TRUE : FALSE;
                        break;
                    case LESS:
                        i3 = i1 < i2 ? TRUE : FALSE;
                        break;
                    default:
                        throw new StoneException("never reach here");
                }
                registers[left] = i3;
            }
            pc += 3;
        }

    }
}
