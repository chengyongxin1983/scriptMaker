using System;

namespace scriptMaker
{
	public class IdToken : Token
	{
		private string value;
		public IdToken (int line, string v):base(line)
		{
			value = v;
		}


		public override bool isId()
		{
			return true;
		}

		public override string getText()
		{
			return value;
		}
	}
}

