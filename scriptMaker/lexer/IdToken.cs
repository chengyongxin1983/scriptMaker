using System;

namespace scriptMaker
{
	public class StrToken : Token
	{
		private string value;
		public StrToken(int line, string v):base(line)
		{
			value = v;
		}


		public override bool isString()
		{
			return true;
		}

		public override string getText()
		{
			return value;
		}


        public override bool isIdentifier() { return true; }
    }
}

