using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine
{
    public class LogicalExpression
    {
        private string _sym, _conn, _original;
        private List<LogicalExpression> _child = new List<LogicalExpression>();

        public LogicalExpression(string sentence)
        {
            _original = sentence;
            sentence = sentence.Trim();
            if (sentence.Contains("<=>") || sentence.Contains("=>") || sentence.Contains("&") || sentence.Contains("-") || sentence.Contains("\\/"))
            {
                SentenceParser(sentence);
                Console.WriteLine("New Sentance = " + sentence);
            }
            else
            {
                Console.WriteLine("Symbol sentence:" + sentence);
                _sym = sentence;
            }
        }

        public LogicalExpression() { }

        public List<LogicalExpression> Child
        {
            get { return _child; }
            set { _child = value; }
        }
        public string Original
        {
            get { return _original; }
        }

        public string Conn
        {
            get { return _conn; }
            set { _conn = value; }
        }

        public string Sym
        {
            get { return _sym; }
        }

        public void SentenceParser(string sentence)
        {
            int operatorCounter = -1;
            int bracketCounter = 0;
            bool trig1 = true, trig2 = true;
            sentence.Trim();

            Console.WriteLine("Original Sentence = " + sentence);
            //Console.Write("Operator Counter: "+ operatorCounter + "\n");
            for(int i = 0; i < sentence.Length; i++)
            {
                char _charachter = sentence.ElementAt(i);
                if(_charachter == '(')
                {
                    bracketCounter++;
                }
                else if(_charachter == ')')
                {
                    bracketCounter--;
                }
                else if (_charachter == '<' && bracketCounter == 0)
                {
                    trig1 = false;
                    trig2 = false;
                    operatorCounter = i;
                }
                else if(_charachter == '=' && _charachter + 1 == '>' && trig2  && bracketCounter == 0)
                {
                    trig1 = false;
                    trig2 = false;
                    operatorCounter = i;
                }
                else if(_charachter == '&' && bracketCounter == 0 && trig1 && trig2)
                {
                    trig1 = false;
                    trig2 = false;
                    operatorCounter = i;
                }
                else if (_charachter == '\\' && bracketCounter == 0 && trig1 && trig2)
                {
                    trig1 = false;
                    trig2 = false;
                    operatorCounter = i;
                }
                else if(_charachter == '~' && bracketCounter == 0 && operatorCounter < 0 && trig1 && trig2)
                {
                    operatorCounter = i;
                }
            }

            if(operatorCounter < 0)
            {
                sentence = sentence.Trim();
                if(sentence.ElementAt(0) == '(' && sentence.ElementAt(sentence.Length - 1) == ')')
                {
                    SentenceParser(sentence.Substring(1 , sentence.Length - 2));
                }
            }
            else
            {
                if(sentence.ElementAt(operatorCounter) == '<')
                {
                    Console.WriteLine("sentence: " + sentence);
                    string str1 = sentence.Substring(0, operatorCounter);
                    str1 = str1.Trim();
                    string str2 = sentence.Substring(operatorCounter + 3);
                    str2 = str2.Trim();
                    LogicalExpression c1 = new LogicalExpression(str1);
                    LogicalExpression c2 = new LogicalExpression(str2);
                    _conn = "<=>";
                    _child.Add(c1);
                    _child.Add(c2);
                }
                else if (sentence.ElementAt(operatorCounter) == '=')
                {
                    string str1 = sentence.Substring(0, operatorCounter);
                    str1 = str1.Trim();
                    string str2 = sentence.Substring(operatorCounter + 2);
                    str2 = str2.Trim();
                    LogicalExpression c1 = new LogicalExpression(str1);
                    LogicalExpression c2 = new LogicalExpression(str2);
                    _conn = "=>";
                    _child.Add(c1);
                    _child.Add(c2);
                }
                else if(sentence.ElementAt(operatorCounter) == '&')
                {
                    string str1 = sentence.Substring(0, operatorCounter);
                    str1 = str1.Trim();
                    string str2 = sentence.Substring(operatorCounter + 1);
                    str2 = str2.Trim();
                    LogicalExpression c1 = new LogicalExpression(str1);
                    LogicalExpression c2 = new LogicalExpression(str2);
                    _conn = "&";
                    _child.Add(c1);
                    _child.Add(c2);
                }
                else if (sentence.ElementAt(operatorCounter) == '\\')
                {
                    string str1 = sentence.Substring(0, operatorCounter);
                    str1 = str1.Trim();
                    string str2 = sentence.Substring(operatorCounter + 2);
                    str2 = str2.Trim();
                    LogicalExpression c1 = new LogicalExpression(str1);
                    LogicalExpression c2 = new LogicalExpression(str2);
                    _conn = "\\/";
                    _child.Add(c1);
                    _child.Add(c2);
                }
                else if (sentence.ElementAt(operatorCounter) == '~')
                {
                    string str1 = sentence.Substring(0, operatorCounter);
                    str1 = str1.Trim();
                    LogicalExpression c1 = new LogicalExpression(str1);
                    _conn = "~";
                    _child.Add(c1);
                }
            }

        }

        public void printInfo()
        {
            Console.WriteLine("Original String: " + _original);
            if (_sym is null)
            {
                if(_child.Count > 1)
                {
                    Console.WriteLine("lEFT = " + _child[0].Original + " CONNECTIVE = " + _conn + "RIGHT: " + _child[1].Original);
                    foreach(LogicalExpression child in _child)
                    {
                        child.printInfo();
                    }
                }
                else if(_child.Count == 1)
                {
                    _child[0].printInfo();
                }
            }
            else
            {
                Console.WriteLine("Symbol = " + _sym);
            }
        }



    }
}
