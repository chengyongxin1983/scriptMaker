using System;
namespace scriptMaker.vm
{
    public class Opcode
    {
        public enum Code
        {


            ICONST = 1,    // load an integer
            BCONST = 2,   // load an 8bit (1byte) integer
            SCONST = 3,    // load a character string
            MOVE = 4,      // move a value
            GMOVE = 5,     // move a value (global variable)
            IFZERO = 6,    // branch if false
            GOTO = 7,      // always branch
            CALL = 8,      // call a function
            RETURN = 9,    // return
            SAVE = 10,     // save all registers
            RESTORE = 11,  // restore all registers
            NEG = 12,      // arithmetic negation
            ADD = 13,      // add
            SUB = 14,      // subtract
            MUL = 15,      // multiply
            DIV = 16,      // divide
            REM = 17,      // remainder
            EQUAL = 18,    // equal
            MORE = 19,     // more than
            LESS = 20,     // less than

    }

        public static byte encodeRegister(int reg)
        {
            if (reg > StoneVM.NUM_OF_REG)
                throw new Exception("too many registers required");
            else
                return (byte)-(reg + 1);
        }

        public static int decodeRegister(byte operand) { return -1 - operand; }

        public static byte encodeOffset(int offset)
        {
            if (offset > Byte.MaxValue)
                throw new Exception("too big byte offset");
            else
                return (byte)offset;
        }

        public static short encodeShortOffset(int offset)
        {
            if (offset < short.MinValue || short.MaxValue < offset)
                throw new Exception("too big short offset");
            else
                return (short)offset;
        }
        public static int decodeOffset(byte operand) { return operand; }
        public static bool isRegister(byte operand) { return operand < 0; }
        public static bool isOffset(byte operand) { return operand >= 0; }
    }
}
