﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;
using static scriptMaker.parser.Parser;

namespace scriptMaker.parser
{
    public class BasicParser
    {
        protected HashSet<String> reserved = new HashSet<String>();
        protected Parser.Operators operators = new Parser.Operators();
        protected Parser primary = null;
        protected Parser factor = null;
        protected Parser expr = null;

        protected Parser statement0 = null;
        protected Parser block = null;
        protected Parser simple = null;
        protected Parser statement = null;
        protected Parser program = null;

        public BasicParser()
        {
            Parser expr0 = Parser.rule();
            primary = Parser.rule(typeof(PrimaryExpr))
              .or(new Parser[] { Parser.rule().sep("(").ast(expr0).sep(")"),
                  Parser.rule().number(typeof(NumberLiteral)),
                  Parser.rule().identifier(typeof(Name), reserved),
                  Parser.rule().stringToken(typeof(StringLiteral)) }).DebugName("primary");
        
            factor = Parser.rule().or(Parser.rule(typeof(NegativeExpr)).sep("-").ast(primary),
                              primary).DebugName("factor");

            expr = expr0.expression(typeof(BinaryExpr), factor, operators).DebugName("express");
            statement0 = Parser.rule().DebugName("statement");
            block = Parser.rule(typeof(BlockStmnt)).sep("{").option(statement0).repeat(Parser.rule().sep(";", Token.EOL).option(statement0)).sep("}");
            block.DebugName("block");
            simple = Parser.rule(typeof(PrimaryExpr)).ast(expr).DebugName("simple");
            statement = statement0.or(new Parser[] { Parser.rule(typeof(IfStmnt)).sep("if").ast(expr).ast(block).option(Parser.rule().sep("else").ast(block)),
                Parser.rule(typeof(WhileStmnt)).sep("while").ast(expr).ast(block),      simple });

            program = Parser.rule().or(statement, Parser.rule(typeof(NullStmnt)))
                           .sep(";", Token.EOL).DebugName("program");

            reserved.Add(";");
            reserved.Add("}");
            reserved.Add(Token.EOL);

            operators.add("=", 1, Operators.RIGHT);
            operators.add("==", 2, Operators.LEFT);
            operators.add(">", 2, Operators.LEFT);
            operators.add("<", 2, Operators.LEFT);
            operators.add("+", 3, Operators.LEFT);
            operators.add("-", 3, Operators.LEFT);
            operators.add("*", 4, Operators.LEFT);
            operators.add("/", 4, Operators.LEFT);
            operators.add("%", 4, Operators.LEFT);
        }
        public ASTree parse(Lexer lexer) 
        {
            return program.parse(lexer);
        }
    }

}
