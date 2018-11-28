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

        string target = "3*2+5;";

        public static void Main (string[] args)
		{
			string[] strs = new string[1];
			strs [0] = target;
			LineNumberReader reader = new LineNumberReader (strs);

			Lexer lexer = new Lexer (reader);
			//Token token = lexer.read ();

			Console.WriteLine ("Hello World!");
            BasicParser bp = new BasicParser();
            while (lexer.peek(0) != Token.EOF)
            {
                ASTree ast = bp.parse(lexer);
                System.Console.Write("=> " + ast.ToString());

                for (int i = 0; i < ast.numChildren(); ++i)
                {
                    int k = (int)ast.child(i).eval(new BasicEnv());
                }
            }

        }
	}
}
