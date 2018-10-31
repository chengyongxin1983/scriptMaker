using System;

namespace scriptMaker
{
	public class Token
	{
		public static Token EOF = new Token(-1);
		public static string EOL = "\\n";

		private int lineNumber;
		public Token (int line)
		{
			lineNumber = line;
		}

		public int getLineNumber()
		{
			return lineNumber;
		}
        
        public virtual  bool isIdentifier() { return false; }
        public virtual bool isId()
		{
			return false;
		}


		public virtual bool isNumber()
		{
			return false;
		}


		public virtual bool isString()
		{
			return false;
		}

		public virtual int getNumber()
		{
			return 0;
		}

		public virtual string getText()
		{
			return "";
		}

	}
}

