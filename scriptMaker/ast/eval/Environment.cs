
using scriptMaker.ast;

public interface Environment {
    void put(string name, object value);
    object get(string name);

    void putNew(string name, object value);
    Environment where(string name);
    void setOuter(Environment e);

    Symbols symbols();
    void put(int nest, int index, object value);
    object get(int nest, int index);
}
