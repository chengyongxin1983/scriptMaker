using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class Name : ASTLeaf
    {
        protected static int UNKNOWN = -1;
        protected int nest;
        int index;

        public Name(Token t):base(t) { index = UNKNOWN; }
        public String name() { return token().getText(); }


        public override object eval(Environment env)
        {
            if (index == UNKNOWN)
                return env.get(name());
            else
                return env.get(nest, index);
        }

        public override void lookup(Symbols syms)
        {
            Symbols.Location loc = syms.get(name());
            if (loc == null)
            {
                throw new Exception("undefine name:" + name());
            }
            else
            {
                nest = loc.nest;
                index = loc.index;
            }
        }

        public void lookupForAssign(Symbols syms)
        {
            Symbols.Location loc = syms.put(name());
            nest = loc.nest;
            index = loc.index;
        }


        public void evalForAssign(Environment env, Object value)
        {
            if (index == UNKNOWN)
                env.put(name(), value);
            else
                env.put(nest, index, value);
        }
    }
}
