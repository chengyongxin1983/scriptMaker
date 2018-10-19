using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.parser
{
    class ParseException : Exception
    {
        public ParseException(String msg) : base(msg)
        {
            
        }
    }
}
      
