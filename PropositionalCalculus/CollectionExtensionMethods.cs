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

            throw new NotImplementedException();
        }

        public static ExpressionOrFormula<T> SolveForElement<T>(this ExpressionOrFormula<T> expressionOrFormula, T element)
        {
            throw new NotImplementedException();
        }

        public static ICollection<T> GetAllElements<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            throw new NotImplementedException();
        }
    }
}
