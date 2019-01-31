using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using static scriptMaker.parser.Parser;

namespace scriptMaker.parser
{

    public class FuncParser : BasicParser
    {
        Parser param;
        Parser parameters;
        Parser paramList;
        Parser def;
        Parser args;
        Parser postfix;

        public FuncParser() : base()
        {
            param = Parser.rule().identifier(reserved);
            parameters = rule(typeof(ParameterList)).ast(param).repeat(rule().sep(",").ast(param));
            paramList = rule().sep("(").maybe(parameters).sep(")");
            def = rule(typeof(DefStmnt)).sep("def").identifier(reserved).ast(paramList).ast(block);
            args = rule(typeof(Arguments)).ast(expr).repeat(rule().sep(",").ast(expr));
            postfix = rule().sep("(").maybe(args).sep(")");

            reserved.Add(")");
            primary.repeat(postfix);
            simple.option(args);
            program.insertChoice(def);
        }
    }
}
