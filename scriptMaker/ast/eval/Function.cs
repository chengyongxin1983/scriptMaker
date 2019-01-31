using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using static scriptMaker.parser.Parser;

namespace scriptMaker.ast
{
    public class Function {
        public ParameterList parameters;
        public BlockStmnt body;
        public Environment env;
        public Function(ParameterList _parameters, BlockStmnt _body, Environment _env) {
            parameters = _parameters;
            body = _body;
            env = _env;
        }
        public Environment makeEnv() { return new NestedEnv(env); }
        public override string ToString() { return "<fun:" + ">"; }
    }
}