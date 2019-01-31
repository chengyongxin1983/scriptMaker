
public interface Environment {
    void put(string name, object value);
    object get(string name);

    void putNew(string name, object value);
    Environment where(string name);
    void setOuter(Environment e);
}
