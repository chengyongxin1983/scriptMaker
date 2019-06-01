using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scriptMaker.ast;

namespace scriptMaker.ast
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

            if (l is PrimaryExpr)
            {
                PrimaryExpr p = l as PrimaryExpr;
                if (p.hasPostfix(0))
                {
                    if (p.postfix(0) is Dot)
                    {
                        object t = p.evalSubExpr(env, 1);
                        if (t is StoneObject)
                        {
                            return setField((StoneObject)t, (Dot)p.postfix(0), rvalue);
                        }
                    }
                    else if (p.postfix(0) is ArrayRef)
                    {
                        object t = p.evalSubExpr(env, 1);
                        if (t is object[])
                        {
                            ArrayRef aref = (ArrayRef)p.postfix(0);
                            object index = (aref.index()).eval(env);
                            if (index is System.Int32)
                            {
                                ((object[])t)[(int)index] = rvalue;
                                return rvalue;
                            }
                        }
                    }
                }
            }
           
             if (l is Name) {
                ((Name)l).evalForAssign(env, rvalue);
                return rvalue;
            }
            else
                throw new ParseException("bad assignment");
        }

        protected object setField(StoneObject obj, Dot expr, object rvalue)
        {
            string name = expr.name();
            try
            {
                obj.Write(name, rvalue);
                return rvalue;
            }
            catch (Exception e)
            {
                throw new ParseException("setField");
            }
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
                return computeAssign(env, right().eval(env));
            }

            object leftObj = left().eval(env);
            object rightObj = right().eval(env);
            return ComputeOp(leftObj, op, rightObj);
        }

        public override void lookup(Symbols syms)
        {
            ASTree leftast = left();
            if ("=" == GetOperator())
            {
                if (leftast is Name) {
                    ((Name)leftast).lookupForAssign(syms);
                    ((ASTree)right()).lookup(syms);
                    return;
                }
            }
            leftast.lookup(syms);
            right().lookup(syms);
        }

    }
}