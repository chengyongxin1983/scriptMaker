

using System.Collections.Generic;

public class BasicEnv : Environment
{
    protected Dictionary<string, object> values = new Dictionary<string, object> ();
    public BasicEnv() { }
    public void put(string name, object value) { values.Add(name, value); }
    public object get(string name) { return values.Remove(name); }
}
