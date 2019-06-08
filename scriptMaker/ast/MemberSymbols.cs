using System;
namespace scriptMaker.ast
{
    public class MemberSymbols : Symbols
    {
        public static int METHOD = -1;
        public static int FIELD = -2;
        protected int type;
        public MemberSymbols(Symbols outer, int type):base(outer)
        {
            this.type = type;
        }
        public override Location get(string key, int nest)
        {
            int index = 0;
            if (table.TryGetValue(key, out index))
            {
                return new Location(type, index);
            }

            if (outer == null)
            {
                return null;
            }

            return outer.get(key, nest);
        }

        public override Location put(string key)
        {
            Location loc = get(key, 0);
            if (loc == null)
                return new Location(type, add(key));
            else
                return loc;
        }
    }
}
