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
            param = Parser.rule().identifier(reserved).DebugName("param");
            parameters = rule(typeof(ParameterList)).ast(param).repeat(rule().sep(",").ast(param)).DebugName("parameters");
            paramList = rule().sep("(").maybe(parameters).sep(")").DebugName("paramList");
            def = rule(typeof(DefStmnt)).sep("def").identifier(reserved).ast(paramList).ast(block).DebugName("def");
            args = rule(typeof(Arguments)).ast(expr).repeat(rule().sep(",").ast(expr)).DebugName("args");
            postfix = rule().sep("(").maybe(args).sep(")").DebugName("postfix");

            reserved.Add(")");
            primary.repeat(postfix);
            primary.insertChoice(rule(typeof(Fun)).sep("fun").ast(paramList).ast(block));

            simple.option(args);
            program.insertChoice(def);

            // class Parser
            Parser member = rule().or(def, simple).DebugName("member");
            Parser class_body = rule(typeof(ClassBody))
                .sep("{").option(member).repeat(rule().sep(";", Token.EOL).option(member)).sep("}").DebugName("classbody");

            Parser defclass = rule(typeof(ClassStmnt)).sep("class").identifier(reserved)
                .option(rule().sep("extend").identifier(reserved)).ast(class_body).DebugName("defclass");

            postfix.insertChoice(rule(typeof(Dot)).sep(".").identifier(reserved));
            program.insertChoice(defclass);

            // array parser
            Parser elements = rule(typeof(Arrayliteral)).ast(expr).repeat(rule().sep(",").ast(expr));
            reserved.Add("]");
            primary.insertChoice(rule().sep("[").maybe(elements).sep("]"));
            postfix.insertChoice(rule(typeof(ArrayRef)).sep("[").ast(expr).sep("]"));


        }
    }
}
