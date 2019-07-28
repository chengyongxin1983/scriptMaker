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

    public class DefStmnt : ASTList
    {
        protected int index;
        protected int size;

        public DefStmnt(List<ASTree> c):base(c) {  }
        public String name() { return ((ASTLeaf)child(0)).token().getText(); }
        public ParameterList parameters() { return (ParameterList)child(1); }
        public BlockStmnt body() { return (BlockStmnt)child(2); }
        public override String ToString()
        {
            return "(def " + name() + " " + parameters() + " " + body() + ")";
        }

        public override Object eval(Environment env)
        {
            Code code = env.code();
            int entry = code.position();
            compile(code);
            env.putNew(name(), new VmFunction(parameters(), body(), env, entry));

            //((Environment)env).put(0, index, new Function(parameters(), body(), env, size));
            return name();
        }

        public override void lookup(Symbols syms)
        {
            index = syms.putNew(name());
            size = Fun.lookup(syms, parameters(), body());
        }

        public int locals() { return size; }
        public void lookupAsMethod(Symbols syms)
        {
            Symbols newSyms = new Symbols(syms);
            newSyms.putNew(SymbolThis.NAME);
            parameters().lookup(newSyms);
            body().lookup(newSyms);
            size = newSyms.size();
        }

        public override void compile(Code c)
        {
            c.nextReg = 0;
            c.frameSize = size + StoneVM.SAVE_AREA_SIZE; // size is argument number

            c.add(Opcode.Code.SAVE);
            c.add(Opcode.encodeOffset(size));
            body().compile(c);
            c.add(Opcode.Code.MOVE);
            c.add(Opcode.encodeRegister((byte)(c.nextReg - 1)));
            c.add(Opcode.encodeOffset(0));
            c.add(Opcode.Code.RESTORE);
            c.add(Opcode.encodeOffset(size));
            c.add(Opcode.Code.RETURN);
        }
    }
}