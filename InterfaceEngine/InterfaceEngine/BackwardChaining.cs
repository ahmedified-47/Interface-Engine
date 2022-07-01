using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine
{
    /*
     * Contains two lists
     * One list is of known facts 
     * other list is of symbols checked
     */
    public class BackwardChaining : Engine
    {
        private KnowlegeBase _kb;
        private string _str;
        private List<string> _symbolChecked = new List<string>();
        private List<string> _knowledgeFacts;

        /*
         * Constructor Backward Chaining
         * fetch fact list from Knowledge Base
         */
        public BackwardChaining(KnowlegeBase kb, string str)
        {
            _kb = kb;
            _str = str;
            _knowledgeFacts = kb.getKnowlegdeFacts();
        }

        /*
         * Initialise a stack to store the start/initial string
         */

        private Stack<string> VSCount(string str)
        {
            Stack<string> stack = new Stack<string>();
            stack.Push(str);
            return stack;
        }

        /*
         * This function prints answer to screen
         * Solve the function calls for BC Algorithm
         */
        public override void Solve()
        {
            string knowledgeFacts = String.Join("; ", _knowledgeFacts.ToArray());
            Console.WriteLine("String Query = " + _str);
            Console.WriteLine("Knowledge Facts = " + knowledgeFacts);
            bool result = PL_BC(_str);
            _symbolChecked.Reverse();
            string entailed = String.Join(",", _symbolChecked.ToArray());
            string answer;
            if (result)
            {
                answer = "Yes";
            }
            else
            {
                answer = "No";
            }
            Console.WriteLine(answer + ": " + entailed);
        }

        /*
         * Main Algorithm for Backward Chaining
         * While vsCount has items in it pops each item out and trim it
         * then add it to the list _symbolChecked
         * Checks if knowledge facts contain that item if so then it add it into the list hav string
         * If have string is empty then returns false
         * ELSE loop through it and push it in vs Count
         */
        public bool PL_BC(string str)
        {
            Stack<string> vsCount = VSCount(str);
            string search;
            while (vsCount.Count != 0)
            {
                search = vsCount.Pop();
                search = search.Trim();

                _symbolChecked.Add(search);
                if (!_knowledgeFacts.Contains(search))
                {
                    List<SentenceClause> _haveString = new List<SentenceClause>();
                    foreach (SentenceClause sentenceClause in _kb.SentenceClauses)
                    {
                        if (sentenceClause.ConclusionPart.Contains(search))
                        {
                            _haveString.Add(sentenceClause);
                        }
                    }
                    if (_haveString.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (SentenceClause sentenceClause in _haveString)
                        {
                            foreach (string a in sentenceClause.PremisePart)
                            {
                                if (!_symbolChecked.Contains(a))
                                {
                                    vsCount.Push(a);
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

    }
}