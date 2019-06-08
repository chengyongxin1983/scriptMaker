using System;
namespace scriptMaker.ast
{
    public class SymbolThis : Symbols
    {
        public SymbolThis()
        {
        }

        public static string NAME = "this";
        public SymbolThis(Symbols outer):base(outer)
        {
            add(NAME);
        }
        public override int putNew(string key)
        {
            throw new Exception("fatal");
        }
        public override Location put(string key)
        {
            Location loc = outer.put(key);
            if (loc.nest >= 0)
                loc.nest++;
            return loc;
        }
    }
}
