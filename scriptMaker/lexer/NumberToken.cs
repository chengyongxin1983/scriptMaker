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

		public override int getNumber()
		{
			return value;
		}

		public override string getText()
		{
			return value.ToString();
		}
	}
}

