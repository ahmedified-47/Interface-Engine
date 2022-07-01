using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine
{
    public class ForwardChaining : Engine
    {
            private KnowlegeBase _kb;
            private string _str;
            private Dictionary<string, bool> _inferredPairs;

        /*
         * Construcutor for Forward Chaining Algorithm
         */
        public ForwardChaining(KnowlegeBase kb, string str) 
        {
            _kb = kb;
            _str = str;
        }

        /*
         * Prints the list of symbols that were inferred when program was searching for a goal
         * Function prints true/false depending on answer
         */
        public override void Solve()
        {
            bool result = PL_FC();
            string answer , chainingPath = "";

            if (result)
            {
                foreach(KeyValuePair<string, bool> pair in _inferredPairs)
                {
                    if (pair.Value)
                    {
                        chainingPath = chainingPath + pair.Key + ",";
                    }
                }
            }
            if (result)
            {
                answer = "Yes";
            }
            else
            {
                answer = "No";
            }
            Console.WriteLine(answer + ":- " + chainingPath);
        }
        /*
         * Initialise queue strings that are symbols to compare
         * Finds out if sentence clause in knowledge base can be found out by checking
         * If there is no premis(sentence)
         * If it is true then sentence clause is added to the result queue
         */
        private Queue<string> VSCount()
        {
            Queue<string> result = new Queue<string>();
            foreach(SentenceClause sentenceClause in _kb.SentenceClauses)
            {
                if(sentenceClause.PremisePart == null)
                {
                    result.Enqueue(sentenceClause.ConclusionPart);
                }
            }
            return result;
        }
        /*
         * Intitialize a dictionary
         * Dictionary determines when a clause has benn resolved
         * Initialise count for sentence clause
         * with the number of items in its premise(sentence)
         */
        private Dictionary<SentenceClause , int> KeyValueCount()
        {
            Dictionary<SentenceClause ,int> result = new Dictionary<SentenceClause ,int>();
            foreach(SentenceClause sentenceClause in _kb.SentenceClauses)
            {
                if(sentenceClause.PremisePart != null)
                {
                    result[sentenceClause] = sentenceClause.PremisePart.Count;
                }
            }
            return result;
        }
        /*
         * Initialise all symbols to false and determines 
         * if a symbol has been inferred
         */
        public Dictionary<string, bool> InferredPairs()
        {
            Dictionary<string, bool> result = new Dictionary<string,bool>();
            List<string> signs = _kb.getSign();
            foreach(string sign in signs)
            {
                result[sign] = false;
            }
            return result;
        }

        /*
         * While vs has items in it, it takes the first element checks IF the element is the query
         * ELSE looks that element has already been inferred
         * IF NOT it looks for any sentence clause that contains the item
         * if element is found it reduces the sentence clause count.
         * IF sentence clause count == 0 then summary/conclusion is added again to the vs
         * When vs is empty returns false as string not found
         */
        public bool PL_FC()
        {
            Queue<string> vs = VSCount();
            Dictionary<string, bool> inferredPairs = InferredPairs();
            Dictionary<SentenceClause, int> keyValueCount = KeyValueCount(); 

            while (vs.Count != 0)
            {
                string sym = vs.Dequeue();

                if(sym == _str)
                {
                    _inferredPairs = inferredPairs;
                    _inferredPairs[sym] = true;
                    return true;
                }

                if (inferredPairs[sym] == false)
                {
                    inferredPairs[sym] = true ;
                    foreach(SentenceClause _sentenceClause in _kb.SentenceClauses)
                    {
                        if(_sentenceClause.PremisePart != null)
                        {
                            if (_sentenceClause.PremisePart.Contains(sym))
                            {
                                keyValueCount[_sentenceClause]--;
                                if(keyValueCount[_sentenceClause] == 0)
                                {
                                    vs.Enqueue(_sentenceClause.ConclusionPart);
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
