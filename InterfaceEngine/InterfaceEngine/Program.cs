using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static KnowlegeBase _kb;
        static AdvanceKnowlegeBase _akb;
        static string _query;
        static Engine _engine;
        static void Main(string[] args)
        {
            //format: iengine method filename
            try
            {
                ReadProblem(args[1], args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Mode: " + args[0]);

            switch (args[0])
            {
                case ("TT"):
                    _engine = new TruthTable(_kb, _query);
                    break;
                case ("BC"):
                    _engine = new BackwardChaining(_kb, _query);
                    break;
                case ("FC"):
                    _engine = new ForwardChaining(_kb, _query);
                    break;
                case ("DPLL"):
                    _engine = new DPLL();
                    break;
                default:
                    throw new ArgumentException("No valid inference methid given");
            }
            _engine.Solve();
        }

        public static bool ReadProblem(string filename, string solver)
        {
            List<string> text = new List<string>();

            //tries to read problem file, if it can't returns false
            try
            {
                StreamReader reader = File.OpenText(filename);
                for (int i = 0; i < 4; i++)
                {
                    text.Add(reader.ReadLine());
                }
                reader.Close();
            }
            catch
            {
                Console.WriteLine("Error: Could not read file");
                return false;
            }
            string[] knowledge = text[1].Split(';');
            knowledge = knowledge.Take(knowledge.Count() - 1).ToArray();
            List<SentenceClause> clauses = new List<SentenceClause>();
            // If basic checking method 
            if (solver != "GTT" || solver != "DPLL")
            {
                foreach (string s in knowledge)
                {
                    if (s.Contains("=>"))
                    {
                        List<string> premiseSymbols = new List<string>();
                        int index = s.IndexOf("=>");
                        string premise = s.Substring(0, index);
                        string conclusion = s.Substring(index + 2);
                        conclusion = conclusion.Trim();
                        string[] splitPremise = { "" };
                        if (premise.Contains("&"))
                        {
                            splitPremise = premise.Split('&');
                        }
                        else
                        {
                            splitPremise[0] = premise;
                        }
                        foreach (string symbol in splitPremise)
                        {
                            string trim = symbol.Trim();
                            premiseSymbols.Add(trim);
                        }
                        clauses.Add(new SentenceClause(premiseSymbols, conclusion));
                    }
                    else
                    {
                        string conclusion = s.Trim();
                        clauses.Add(new SentenceClause(conclusion));
                    }
                }
                _query = text[3];
                _kb = new KnowlegeBase(clauses);


                return true;
            }
            // if solving method more advanced clauses
            else
            {
                List<LogicalExpression> kb = new List<LogicalExpression>();
                foreach (string s in knowledge)
                {

                    LogicalExpression exp = new LogicalExpression(s);
                    //exp.printInfo();
                    kb.Add(exp);
                }

                _akb = new AdvanceKnowlegeBase(kb);
                _query = text[3];
                return true;
            }


        }

    }
}