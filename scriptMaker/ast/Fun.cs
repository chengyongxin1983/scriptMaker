using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
{

    public class Fun : ASTList
    {
        public Fun(List<ASTree> c) : base(c) { }

        public  ParameterList parameters() { return (ParameterList)child(0); }

        public  BlockStmnt body() { return (BlockStmnt)child(1); }

        public override string toString()
        {
            return "(fun" + parameters() + body() + ")";
        }

        public override Object eval(Environment env)
        {
            return new Function(parameters(), body(), env);
        }
    }
}