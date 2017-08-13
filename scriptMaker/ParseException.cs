using System;
using System.IO;
namespace scriptMaker
{
	public class ParseException : Exception {
	    public ParseException(Token t) {
			
	    }
		public ParseException(string msg, Token t): base("syntax error around " + location(t) + ". " + msg) {
	       
	    }
	    private static String location(Token t) {
	        if (t == Token.EOF)
	            return "the last line";
	        else
	            return "\"" + t.getText() + "\" at line " + t.getLineNumber();
	    }
	    public ParseException(IOException e) {
	       
	    }
	    public ParseException(string msg) {
	     
	    }
	}
}
