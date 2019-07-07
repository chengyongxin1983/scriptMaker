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

     encodeRegister(int reg)
        {
            if (reg > StoneVM.NUM_OF_REG)
                throw new StoneException("too many registers required");
            else
                return (byte)-(reg + 1);
        }
        public static int decodeRegister(byte operand) { return -1 - operand; }
         encodeOffset(int offset)
        {
            if (offset > Byte.MAX_VALUE)
                throw new StoneException("too big byte offset");
            else
                return (byte)offset;
        }
        public static short encodeShortOffset(int offset)
        {
            if (offset < Short.MIN_VALUE || Short.MAX_VALUE < offset)
                throw new StoneException("too big short offset");
            else
                return (short)offset;
        }
        public static int decodeOffset(byte operand) { return operand; }
        public static boolean isRegister(byte operand) { return operand < 0; }
        public static boolean isOffset(byte operand) { return operand >= 0; }
    }
}
