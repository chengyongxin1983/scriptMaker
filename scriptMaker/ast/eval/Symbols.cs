using System;
using System.Collections.Generic;

namespace scriptMaker.ast
{
    public class Symbols
    {
        public class Location
        {
            public int nest;
            public int index;
            public Location(int _nest, int _index)
            {
                nest = _nest;
                index = _index;
            }
        }

        protected Symbols outer;
        protected Dictionary<string, int> table;

        public Symbols()
        {
            table = new Dictionary<string, int>();
        }

        public Symbols(Symbols _outer)
        {
            outer = _outer;
            table = new Dictionary<string, int>();
        }

        public int size() { return table.Count; }

        public void append(Symbols s)
        {
            foreach (var t in s.table)
            {
                table.Add(t.Key, t.Value);
            }
        }

        public int find(string key, out bool result)
        {
            int value = 0;
            if (string.IsNullOrEmpty(key))
            {
                result = false;
                return value;
            }
            result = table.TryGetValue(key, out value);
            return value;
        }

        public Location get(string key)
        {
            return get(key, 0);
        }

        public Location get(string key, int nest)
        {
            int value = 0;
            if (table.TryGetValue(key, out value))
            {
                return new Location(nest, value);
            }

            if (outer == null)
            {
                return null;
            }

            return outer.get(key, nest + 1);
        }

        protected int add(string key)
        {
            int i = table.Count;
            table.Add(key, i);
            return i;
        }

        public int putNew(string key)
        {
            int value = 0;
            if (!table.TryGetValue(key, out value))
            {
                return add(key);
            }

            return value;
        }

        public Location put(string key)
        {
            Location loc = get(key, 0);

            if (loc == null)
            {
                return new Location(0, add(key));
            }

            return loc;
        }

    }
}
