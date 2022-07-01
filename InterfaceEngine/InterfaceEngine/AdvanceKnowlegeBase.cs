using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine
{
    public class AdvanceKnowlegeBase
    {
        private List<LogicalExpression> _expression;

        public List<LogicalExpression> Expression => _expression;
        public AdvanceKnowlegeBase(List<LogicalExpression> expressions)
        {
            _expression = expressions;
        }
    }
}