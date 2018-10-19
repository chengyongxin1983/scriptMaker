﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace scriptMaker.ast
{
    public class ASTList : ASTree
    {
        protected List<ASTree> _children;
        public ASTList(List<ASTree> list) { _children = list; }
        public override ASTree child(int i) { return _children[i]; }
        public override int numChildren() { return _children.Count; }
        public override IEnumerator<ASTree> children() { return _children.GetEnumerator(); }
        public String toString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            String sep = "";
            foreach (ASTree t in _children)
            {
                builder.Append(sep);
                sep = " ";
                builder.Append(t.ToString());
            }
            return builder.Append(')').ToString();
        }
        public override String location()
        {
            foreach (ASTree t in _children)
            {
                String s = t.location();
                if (s != null)
                    return s;
            }
            return null;
        }
    }

}