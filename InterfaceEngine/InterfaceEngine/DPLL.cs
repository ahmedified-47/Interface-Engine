using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine
{
    public class DPLL : Engine
    {
        public DPLL()
        {

        }
        public override void Solve()
        {
            bool result;
            LogicalExpression logicalExpression = new LogicalExpression("(a&b)<=>c");
            logicalExpression = CNF_convert(logicalExpression);
            logicalExpression.printInfo();
            result = IS_TRUE(logicalExpression);
          //  Console.WriteLine(logicalExpression);
            Console.WriteLine(result);
        }
        public bool IS_TRUE(LogicalExpression logicalExpression)
        {
            if (logicalExpression != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public LogicalExpression CNF_convert(LogicalExpression alogicalExpression)
        {
            if (alogicalExpression.Sym != null)
            {
                return alogicalExpression;
            }
            else if (alogicalExpression.Conn == "<=>")
            {
                LogicalExpression _logicalExpression = new LogicalExpression();
                _logicalExpression.Conn = "&";
                LogicalExpression child_1 = new LogicalExpression(alogicalExpression.Child[0].Original);
                LogicalExpression child_2 = new LogicalExpression(alogicalExpression.Child[1].Original);

                List<LogicalExpression> childList = new List<LogicalExpression>();
                childList.Add(child_1);
                childList.Add(child_2);
                _logicalExpression.Child = childList;
                Console.WriteLine("Remove Bi-Implication = ");
                _logicalExpression.printInfo();
                return _logicalExpression;

            }
            else if(alogicalExpression.Conn == "=>")
            {
                LogicalExpression _logicalExpression = new LogicalExpression();
                _logicalExpression.Conn = "\\/";
                LogicalExpression child_1 = CNF_convert(alogicalExpression.Child[0]);
                LogicalExpression child_2 = CNF_convert(alogicalExpression.Child[1]);
                LogicalExpression negate = new LogicalExpression();
                negate.Conn = "~";
                negate.Child.Add(child_1);
                _logicalExpression.Child.Add(negate);
                //negate.Child.Add(child_2);
                _logicalExpression.Child.Add(child_2);
                Console.WriteLine("Remove Implication: ");
                _logicalExpression.printInfo();
                return (_logicalExpression);
            }
            else if(alogicalExpression.Conn == "~")
            {
                if(alogicalExpression.Child[0].Sym != null)
                {
                    return alogicalExpression;
                }
                else
                {
                    if(alogicalExpression.Child[0].Conn == "\\/")
                    {
                        LogicalExpression _logicalExpression = new LogicalExpression();
                        LogicalExpression child_1 = new LogicalExpression();
                        LogicalExpression child_2 = new LogicalExpression();
                        _logicalExpression.Conn = "&";
                        child_1.Conn = "~";
                        child_2.Conn = "~";
                        child_1.Child.Add(alogicalExpression.Child[0].Child[0]);
                        child_2.Child.Add(alogicalExpression.Child[0].Child[1]);
                        _logicalExpression.Child.Add(child_1);
                        _logicalExpression.Child.Add(child_2);
                        return _logicalExpression;
                    }
                    else if(alogicalExpression.Child[0].Conn == "&")
                    {
                        LogicalExpression _logicalExpression = new LogicalExpression();
                        LogicalExpression child_1 = new LogicalExpression();
                        LogicalExpression child_2 = new LogicalExpression();
                        _logicalExpression.Conn = "\\/";
                        child_1.Conn = "~";
                        child_2.Conn = "~";
                        child_1.Child.Add(alogicalExpression.Child[0].Child[0]);
                        child_2.Child.Add(alogicalExpression.Child[0].Child[1]);
                        _logicalExpression.Child.Add(child_1);
                        _logicalExpression.Child.Add(child_2);
                        return _logicalExpression;
                    }
                }
            }
            return alogicalExpression;
        }
    }
}
