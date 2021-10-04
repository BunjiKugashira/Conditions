namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class CollectionExtensionMethods
    {
        public static IDictionary<T, ExpressionOrFormula<T>> SolveForEachElement<T>(this ExpressionOrFormula<T> expressionOrFormula, params T[] elements)
        {
            if (!elements?.Any() ?? true)
            {
                elements = expressionOrFormula.GetAllElements().ToArray();
            }

            return elements.ToDictionary(element => element, element => SolveForElement(expressionOrFormula, element));
        }

        public static ExpressionOrFormula<T> SolveForElement<T>(this ExpressionOrFormula<T> expressionOrFormula, T element)
        {
            throw new NotImplementedException();
        }

        public static ISet<T> GetAllElements<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            if (expressionOrFormula is Expression<T> expression)
            {
                return new HashSet<T>() { expression.Value };
            }
            else if (expressionOrFormula is Formula<T> formula)
            {
                return formula.ExpressionOrFormulas.SelectMany(GetAllElements).ToHashSet();
            }
            else
            {
                throw new NotImplementedException("Method was not implemented for the given type.");
            }
        }
    }
}
