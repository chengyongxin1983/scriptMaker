using scriptMaker.ast;
using scriptMaker.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace scriptMaker
{


    class MainClass
    {   
        /// FileStream读取文件
        public static string[] FileStreamReadFile(string filePath)
        {
            List<string> result = new List<string>();
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Open);
                StreamReader sr = new StreamReader(file);//用FileStream对象实例化一个StreamReader对象
                string strLine = null;//读取完整的文件，如果用这个方法，就可以不用下面的while循环

                do
                {
                    strLine = sr.ReadLine();
                    if (strLine != null)
                    {
                        result.Add(strLine);
                    }
                }
                while (strLine != null);

                sr.Close();
                file.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result.ToArray();
        }

        private static
        //string target = "if (i==\"DSAFG\")";

        string target = "def fun(i)"
            + "\n{ r = 1; if i == 1 "
            + "\n {r=1;} "
            + "\n else {r = fun(i-1) * i;}  };"
            + "\nk=fun(5);";
        //string target = "def fun(i)"
        //    + "\n{ r = 1; "
        //    + "\n r = 2;};"
        //    + "\n k=fun(2);";
        public static void Main (string[] args)
		{
			string[] strs = FileStreamReadFile("test.txt");
			//strs [0] = target;
			LineNumberReader reader = new LineNumberReader (strs);

			Lexer lexer = new Lexer (reader);
			//Token token = lexer.read ();

			Console.WriteLine ("Hello World!");
            FuncParser bp = new FuncParser();

            NestedEnv env = new NestedEnv();
            while (lexer.peek(0) != Token.EOF)
            {
                ASTree ast = bp.parse(lexer);
                if (!(ast is NullStmnt)) {
                    object r = ast.eval(env);
                    
                }
            }

        }
	}
}
