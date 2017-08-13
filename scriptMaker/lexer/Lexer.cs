using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace scriptMaker
{
	public class Lexer 
	{
		public static string regexPat
		= "\\s*((//.*)|([0-9]+)|(\"(\\\\\"|\\\\\\\\|\\\\n|[^\"])*\")"
			+ "|[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\\|\\||[!\\\\\\\"#\\$%&'\\(\\)*\\+,-\\./:;<=>\\?@\\[\\\\\\\\\\]\\^_`\\{\\|\\}~])?";
		private Regex pattern = new Regex(regexPat, RegexOptions.Compiled);
		private List<Token> queue = new List<Token>();
		private bool hasMore;
		private LineNumberReader reader;

		public Lexer(LineNumberReader r) {
			hasMore = true;
			reader = r;
		}
		public Token read() {
			Token result = null;
			if (fillQueue (0)) {
				result = queue [0];
				queue.RemoveAt (0);
			}
			else
				result = Token.EOF;
			return result;
		}
		public Token peek(int i) {
			Token result = null;
			if (fillQueue(i)) {
				result = queue [0];
				queue.RemoveAt (0);
			}
			else
				result = Token.EOF;
			return result;
		}
		private bool fillQueue(int i) {
			while (i >= queue.Count)
				if (hasMore)
					readLine();
				else
					return false;
			return true;
		}
		protected void readLine() {
			string line;
			try {
				line = reader.ReadLine();
			} catch (IOException e) {
				throw new ParseException(e);
			}
			if (line == null) {
				hasMore = false;
				return;
			}
			int lineNo = reader.GetLineNumber();
			MatchCollection matchers = pattern.Matches(line);

			int iMatch = 0;
			foreach (Match match in matchers)
			{
//				Console.Write ("match [" + iMatch.ToString () + "]\n");
//				for (int i = 0; i < match.Groups.Count; ++i)
//				{
//					Console.Write ("\t: Group(" + i.ToString () + "):" + match.Groups [i].Value + "\n");
//				}
//				iMatch++;
				addToken(lineNo, match);
			}
			//matcher.useTransparentBounds(true).useAnchoringBounds(false);

			/* 
			int pos = 0;
			int endPos = line.length();
			while (pos < endPos) {
				matcher.region(pos, endPos);
				if (matcher.lookingAt()) {
					addToken(lineNo, matcher);
					pos = matcher.end();
				}
				else
					throw new ParseException("bad token at line " + lineNo);
			}
			queue.add(new IdToken(lineNo, Token.EOL));
			*/
		}


		protected void addToken(int lineNo, Match matcher) {
			if (matcher.Groups.Count > 1)
			{
				string m = matcher.Groups[1].Value;
				if (!string.IsNullOrEmpty(m)) { // if not a space
					if (string.IsNullOrEmpty(matcher.Groups [2].Value)) { // if not a comment
						Token token;
						if (!string.IsNullOrEmpty(matcher.Groups [3].Value))
							token = new NumberToken (lineNo, int.Parse (matcher.Groups [3].Value));
						else if (!string.IsNullOrEmpty(matcher.Groups [4].Value ))
							token = new StrToken (lineNo, toStringLiteral (matcher.Groups [4].Value));
						else
							token = new IdToken (lineNo, m);
						queue.Add (token);
					}
				}
			}

		}



		protected String toStringLiteral(string s) {
			StringBuilder sb = new StringBuilder();
			int len = s.Length - 1;
			for (int i = 1; i < len; i++) {
				char c = s[i];
				if (c == '\\' && i + 1 < len) {
					int c2 = s[i + 1];
					if (c2 == '"' || c2 == '\\')
						c = s[++i];
					else if (c2 == 'n') {
						++i;
						c = '\n';
					}
				}
				sb.Append(c);
			}
			return sb.ToString();
		}

	
	}

}

