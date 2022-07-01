using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine
{
    public class SentenceClause
    {
        private List<string> premise_part;
        private string conclusion_part;

        public SentenceClause(string aConclusion)
        {
            conclusion_part = aConclusion;
        }

        public SentenceClause(List<string> aPremise , string aConclusion)
        {
            premise_part = aPremise;
            conclusion_part = aConclusion;
        }
        public List<string> PremisePart
        {
            get { return premise_part; }
        }

        public string ConclusionPart
        {
            get { return conclusion_part; }
        }
    }
}
