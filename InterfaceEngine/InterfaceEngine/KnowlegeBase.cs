using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace InterfaceEngine
{
    public class KnowlegeBase //Simple knowlegde base
    {
        private List<SentenceClause> _sentenceClauses;

        public KnowlegeBase(List<SentenceClause> sentenceClauses)
        {
            _sentenceClauses = sentenceClauses;
        }

        public List<SentenceClause> SentenceClauses
        {
            get { return _sentenceClauses; }
        }

        public List<string> getSign() // getting sign/symbol stred in the knowledge base
        {
            List<string> sign = new List<string>();
            foreach (SentenceClause sentenceClause in _sentenceClauses)
            {
                if (sentenceClause.PremisePart != null) // if there is a sentence in a clause
                {
                    foreach (string word in sentenceClause.PremisePart)
                    {
                        if (!sign.Contains(word))
                        {
                            sign.Add(word);
                        }
                    }
                }
                if (!sign.Contains(sentenceClause.ConclusionPart)) // check if sybol already contains the solution
                {
                    sign.Add(sentenceClause.ConclusionPart);
                }
            }
            return sign;
        }

        public List<string> getKnowlegdeFacts() // returns list of facts that arknown from th knowledge base
        {
            List<string> sign = new List<string>();
            foreach (SentenceClause sentenceClause in _sentenceClauses)
            {
                if (sentenceClause.PremisePart is null)
                {
                    sign.Add(sentenceClause.ConclusionPart);
                }
            }
            return sign;
        }

        //gets a list of unique symbols in kb
        public List<string> getSymbols()
        {
            List<string> symbols = new List<string>();
            /* This regex can match -
               * at least one letter and one or no numbers, e.g p2, h, a
               * the symbols =>, &, ~, ||, or <=>
            */
            string regexPattern = @"[A-Za-z]+[0-9]*|=>|&|~|\|\||<=>";
            string connectives = @"=>|&|~|\|\||<=>";
            foreach (SentenceClause sentenceClause in _sentenceClauses)
            {
                //goes through each string that matches the regex
                foreach (Match m in Regex.Matches(sentenceClause.ConclusionPart, regexPattern))
                {
                    // gets the multiple matches within each string match
                    foreach (Capture capture in m.Captures)
                    {
                        // prints out all the individual matches
                        //Console.WriteLine("Index= {0}, Value= {1}", capture.Index, capture.Value);

                        //get only symbols and add them to list
                        if (!Regex.IsMatch(capture.Value, connectives))
                        {
                            if (symbols.Find(i => i == capture.Value) == null)
                            {
                                symbols.Add(capture.Value);
                            }
                        }
                    }
                }
            }
            return symbols;
        }

        //checks if KB is true given a model
        public bool checkTrue(Dictionary<string, bool> model)
        {
            string regexPattern = @"[A-Za-z]+[0-9]*|=>|&|~|\|\||<=>";
            string connectives = @"=>|&|~|\|\||<=>";
            List<string> equation = new List<string>();

            bool result = false;
            string op = "none";

            //goes through each sentence of kb and calculates whether it is true or false.
            foreach (SentenceClause clause in _sentenceClauses)
            {
                foreach (Match m in Regex.Matches(clause.ConclusionPart, regexPattern))
                {
                    foreach (Capture capture in m.Captures)
                    {
                        //checks if the captured string is a connective
                        if (!Regex.IsMatch(capture.Value, connectives))
                        {
                            //find symbol bool in model and update result
                            bool nextSymbol = model[capture.Value];
                            // "none" is only in the beginning, if its false then return false for optimisation
                            switch (op)
                            {
                                case "none":
                                    result = nextSymbol;
                                    if (!result)
                                    {
                                        return false;
                                    }
                                    break;
                                case "&":
                                    result = result && nextSymbol;
                                    break;
                                case "=>":
                                    if (result)
                                    {
                                        result = nextSymbol;
                                        if (!result)
                                        {
                                            return false;
                                        }
                                    }
                                    else { result = true; }
                                    break;
                                default:
                                    Console.WriteLine("symbol unrecognised");
                                    break;
                            }
                        }
                        else
                        {
                            //change operator mode if reads in connective
                            op = capture.Value;
                        }
                    }
                }
            }
            return result;
        }
    }
}