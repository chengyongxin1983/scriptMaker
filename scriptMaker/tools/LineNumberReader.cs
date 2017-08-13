using System;

namespace scriptMaker
{
	public class LineNumberReader
	{
		int lineCount = 0;
		string[] _lines;
		public LineNumberReader (string[] lines)
		{
			_lines = lines;
			lineCount = 0;
		}

		public string ReadLine()
		{
			string result = null;

			if (lineCount < _lines.Length)
			{
				result = _lines [lineCount];
				lineCount++;
			}

			return result;
		}

		public int GetLineNumber()
		{
			return lineCount;
		}

	}
}

