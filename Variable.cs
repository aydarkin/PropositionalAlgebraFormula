using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropositionalAlgebraFormula
{
    /// <summary>
    /// Переменная алгебры высказываний
    /// </summary>
    public class Variable
    {
        public string View { get; set; }
        public bool IsNegative { get; set; }

        public Variable(string variable)
        {
            if (variable.Contains(Formula.AccessNegativeOperator))
            {
                IsNegative = true;
                View = variable.Replace(Formula.AccessNegativeOperator.ToString(), "");
            }
            else
            {
                IsNegative = false;
                View = variable;
            }    
        }

        public Variable(string view, bool isNegative)
        {
            this.View = view;
            this.IsNegative = isNegative;
        }

        public Variable(Variable variable)
        {
            this.View = variable.View;
            this.IsNegative = variable.IsNegative;
        }

        public override string ToString()
        {
            if (IsNegative)
                return Formula.AccessNegativeOperator + this.View;
            else
                return this.View;
        }
    }
}
