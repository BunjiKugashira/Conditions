namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using PropositionalCalculus.BinaryOperators;

    public static class NormalizationExtensionMethods
    {
        public static ExpressionOrFormula<T> ToConjunctiveNormalForm<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            throw new NotImplementedException();
        }

        public static bool IsConjunctiveNormalForm<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            throw new NotImplementedException();
        }

        public static ExpressionOrFormula<T> ToDisjunctiveNormalForm<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            throw new NotImplementedException();
        }

        public static bool IsDisjunctiveNormalForm<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            throw new NotImplementedException();
        }

        public static ExpressionOrFormula<T> ToNegationNormalForm<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            throw new NotImplementedException();
        }

        public static bool IsNegationNormalForm<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            throw new NotImplementedException();
        }

        public static ExpressionOrFormula<T> ToCanonicalNormalForm<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            throw new NotImplementedException();
        }

        public static bool IsCanonicalNormalForm<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            throw new NotImplementedException();
        }

        public static ExpressionOrFormula<T> RemoveRedundantParenthesis<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            if (expressionOrFormula is Formula<T> formula)
            {
                if (!formula.ExpressionOrFormulas.Any())
                {
                    return new TruthValue<T>(formula.BinaryOperator, formula.UnaryOperators, true);
                }

                if (formula.ExpressionOrFormulas.Count() == 1)
                {
                    var first = formula.ExpressionOrFormulas.First().RemoveRedundantParenthesis();
                    var unaryOperators = formula.UnaryOperators.ToList();
                    unaryOperators.AddRange(first.UnaryOperators);
                    return first.WithOperators(formula.BinaryOperator, unaryOperators);
                }

                var eofs = formula.ExpressionOrFormulas.Select(eof => eof.RemoveRedundantParenthesis()).ToArray();
                var combinedEofs = new List<ExpressionOrFormula<T>>();
                for (var i = 0; i < eofs.Length; i++)
                {
                    if (eofs[i] is Formula<T> innerFormula && innerFormula.IsRedundant(eofs.ElementAtOrDefault(i + 1)?.BinaryOperator))
                    {
                        var subEofs = innerFormula.ExpressionOrFormulas.Select(eof => 
                        {
                            var unaryOperators = innerFormula.UnaryOperators.ToList();
                            unaryOperators.AddRange(eof.UnaryOperators);
                            return eof.WithOperators(eof.BinaryOperator, unaryOperators);
                        }).ToArray();
                        subEofs[0] = subEofs[0].WithOperators(innerFormula.BinaryOperator, subEofs[0].UnaryOperators);
                        combinedEofs.AddRange(subEofs);
                    }
                    else
                    {
                        combinedEofs.Add(eofs[i]);
                    }
                }

                return new Formula<T>(formula.BinaryOperator, formula.UnaryOperators, combinedEofs.ToArray());
            }

            return expressionOrFormula;
        }

        public static bool IsRedundant<T>(this Formula<T> formula, BinaryOperator nextOperator)
        {
            var mightBeRedundant = true;

            if (formula.BinaryOperator != null)
            {
                mightBeRedundant &= formula.ExpressionOrFormulas.All(eof => (eof.BinaryOperator?.CompareTo(formula.BinaryOperator) ?? -1) <= 0);
            }

            if (nextOperator != null)
            {
                mightBeRedundant &= formula.ExpressionOrFormulas.All(eof => (eof.BinaryOperator?.CompareTo(nextOperator) ?? -1) <= 0);
            }

            return mightBeRedundant;
        }

        public static bool HasRedundantParenthesis<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            if (expressionOrFormula is Formula<T> formula)
            {
                var eofs = formula.ExpressionOrFormulas.ToArray();

                if (eofs.Length < 2)
                {
                    return true;
                }

                for (var i = 0; i < eofs.Length; i++)
                {
                    if (eofs[i] is Formula<T> innerFormula && innerFormula.IsRedundant(eofs.ElementAtOrDefault(i + 1)?.BinaryOperator))
                    {
                        return true;
                    }
                }

                if (eofs.Any(eof => eof.HasRedundantParenthesis()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
