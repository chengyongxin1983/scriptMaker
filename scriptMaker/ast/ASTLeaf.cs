using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{
    public class ASTLeaf : ASTree
    {
        private static List<ASTree> empty = new List<ASTree>();
        protected Token _token;
        public ASTLeaf(Token t) { _token = t; }
        public override ASTree child(int i) { throw new Exception(); }
        public override int numChildren() { return 0; }
        public override IEnumerator<ASTree> children() { return empty.GetEnumerator(); }
        public String toString() { return _token.getText(); }
        public override String location() { return "at line " + _token.getLineNumber(); }
        public Token token() { return _token; }
    }

}
