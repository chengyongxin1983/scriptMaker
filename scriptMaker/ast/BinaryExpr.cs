using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.parser
{
    public class BinaryExpr : ASTList
    {
        public BinaryExpr(List<ASTree> c):base(c) {  }
        public ASTree left()
        {
            return child(0);
        }

        public ASTree right() { return child(2); }
        public string GetOperator()
        {
            return ((ASTLeaf)child(1)).token().getText();
        }

        protected Object computeAssign(Environment env, Object rvalue)
        {
            ASTree l = left();
            if (l is Name) {
                env.put(((Name)l).name(), rvalue);
                return rvalue;
            }
            else
                throw new ParseException("bad assignment");
        }
        protected Object computeNumber(System.Int32 left, string op, System.Int32 right)
        {
            int a = left;
            int b = right;
            if (op == "+")
                return a + b;
            else if (op == "-")
                return a - b;
            else if (op == "*")
                return a * b;
            else if (op == "/")
                return a / b;
            else if (op == "%")
                return a % b;
            else if (op == "==")
                return a == b ? true : false;
            else if (op == ">")
                return a > b ? true : false;
            else if (op == "<")
                return a < b ? true : false;
            else
                throw new ParseException("bad operator");
        }
        object ComputeOp(object leftObj, string op, object rightObj)
        {
            if (leftObj is System.Int32 && rightObj is System.Int32) {
                return computeNumber((int)leftObj, op, (int)rightObj);
            }
            else
                if (op == "+")
                return (string)leftObj + (string)rightObj;
            else if (op == "==")
            {
                if (leftObj == null)
                    return rightObj == null ? true : false;
                else
                    return leftObj == rightObj ? true : false;
            }
            else
                throw new ParseException("bad type");
        }
        public override object eval(Environment env)
        {
            string op = GetOperator();
            if (op == "=")
            {
                computeAssign(env, right().eval(env));
            }

            object leftObj = left().eval(env);
            object rightObj = right().eval(env);
            return ComputeOp(leftObj, op, rightObj);
        }
    }
}