using scriptMaker.ast;
using scriptMaker.parser;
using System;
using System.Text.RegularExpressions;

namespace scriptMaker
{
    public class Test
    {
        public virtual void GetM()
        {

        }
    }

	class MainClass
	{

		private static
        //string target = "if (i==\"DSAFG\")";

        string target = "(1+2)";

        public static void Main (string[] args)
		{
			string[] strs = new string[1];
			strs [0] = target;
			LineNumberReader reader = new LineNumberReader (strs);

			Lexer lexer = new Lexer (reader);
			Token token = lexer.read ();

			Console.WriteLine ("Hello World!");
            BasicParser bp = new BasicParser();
            while (lexer.peek(0) != Token.EOF)
            {
                ASTree ast = bp.parse(lexer);
                System.Console.Write("=> " + ast.ToString());
            }

        }
	}
}
