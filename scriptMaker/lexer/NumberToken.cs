using System;

namespace scriptMaker
{
	public class NumberToken : Token
	{
		private int value;
		public NumberToken (int line, int v):base(line)
		{
			value = v;
		}


		public override bool isNumber()
		{
			return true;
		}

		public int getNumber()
		{
			return value;
		}

		public string getText()
		{
			return value.ToString();
		}
	}
}

