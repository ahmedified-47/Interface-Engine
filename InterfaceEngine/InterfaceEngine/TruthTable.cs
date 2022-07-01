using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine
{
    public class TruthTable : Engine
    {
        Dictionary<string, bool> _models;
        List<string> _symbols;
        int trueCount;
        int modelCount;
        bool _debug;

        public TruthTable(KnowlegeBase kb, string query)
        {
            trueCount = 0;
            _symbols = new List<string>();
            modelCount = 0;
            _debug = false; //shows debug info
        }

        //creates symbol list, returns kb entails query
        private bool TTEntails(KnowlegeBase kb, string query)
        {

            _symbols = kb.getSymbols();
            _models = new Dictionary<string, bool>();

            return TTCheckAll(kb, query, _symbols, _models);
        }

        // recursively makes models, once finished it will evaluate each model according to kb
        private bool TTCheckAll(KnowlegeBase kb, string query, List<string> symbols, Dictionary<string, bool> model)
        {
            string p;
            List<string> rest;

            //symbols.count is 0 when the model has been finished
            if (symbols.Count == 0)
            {
                // if kb is true, check if query is true
                //writes out all models
                if (_debug)
                {
                    modelCount++;
                    Console.Write("Model " + modelCount + ": ");
                    foreach (KeyValuePair<string, bool> m in model)
                    {
                        Console.Write(m.Key + ":" + m.Value + ", ");
                    }
                    Console.Write(" : ");
                }
                //check if kb is true, then check if query is true, kb |= query
                if (PL_True(kb, model))
                {
                    if (_debug)
                    {
                        Console.Write("Kb:  True | ");
                    }
                    if (PL_True(query, model))
                    {
                        if (_debug)
                        {
                            Console.WriteLine("query: True.");
                        }
                        trueCount++;
                        return true;
                    }
                    if (_debug)
                    {
                        Console.WriteLine("query: false.");
                    }
                    return false;
                }
                else
                {
                    if (_debug)
                    {
                        Console.WriteLine("Kb:  False");
                    }
                    //implication doesn't care if kb is false
                    return true;
                }
            }
            else
            {
                // get first symbol in list, then remove it from the list
                // then continue making all TT models
                p = symbols[0];

                if (_debug)
                {
                    Console.WriteLine("popped symbol: " + symbols[0]);
                }

                rest = symbols.ToList();
                rest.RemoveAt(0);
                return TTCheckAll(kb, query, rest, Extend(p, true, model)) && TTCheckAll(kb, query, rest, Extend(p, false, model));
            }
        }

        public override void Solve()
        {

            
        }

        // adds a symbol and its bool value to the model
        private Dictionary<string, bool> Extend(string p, bool b, Dictionary<string, bool> model)
        {
            Dictionary<string, bool> ret = CopyDict(model);
            ret.Add(p, b);
            return ret;
        }

        //copies a dictionary
        private Dictionary<string, bool> CopyDict(Dictionary<string, bool> dict)
        {
            Dictionary<string, bool> copy = new Dictionary<string, bool>();
            foreach (KeyValuePair<string, bool> kv in dict)
            {
                copy.Add(kv.Key, kv.Value);
            }
            return copy;
        }

        //checks if KB is true given a model
        private bool PL_True(KnowlegeBase kb, Dictionary<string, bool> model)
        {
            return kb.checkTrue(model);
        }

        //checks if query is true given a model
        private bool PL_True(string query, Dictionary<string, bool> models)
        {
            return models[query];
        }
    }
}