namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using PropositionalCalculus.BinaryOperators;
    using PropositionalCalculus.UnaryOperators;

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
            return expressionOrFormula switch
            {
                Expression<T> => expressionOrFormula.UnaryOperators.Count(o => o is Not) <= 1,
                Formula<T> formula => formula.UnaryOperators.All(o => o is not Not) && formula.ExpressionOrFormulas.All(eof => eof.IsNegationNormalForm()),
                TruthValue<T> => expressionOrFormula.UnaryOperators.All(o => o is not Not),
                _ => throw new NotImplementedException("This method is not implemented for type " + expressionOrFormula.GetType())
            };
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

        public static ExpressionOrFormula<T> Flatten<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            if (expressionOrFormula is Formula<T> formula)
            {
                if (!formula.ExpressionOrFormulas.Any())
                {
                    return formula;
                }

                if (formula.ExpressionOrFormulas.Count() == 1)
                {
                    var single = formula.ExpressionOrFormulas.Single();
                    var unaryOperators = formula.UnaryOperators.ToList();
                    unaryOperators.AddRange(single.UnaryOperators);
                    return single.WithOperators(formula.BinaryOperator, unaryOperators);
                }

                var eofs = formula.ExpressionOrFormulas.Select(Flatten);
                var first = eofs.First();
                var result = first is Formula<T> firstFormula ? firstFormula : new Formula<T>(first.BinaryOperator, first.WithOperators(null, first.UnaryOperators));
                foreach(var eof in eofs.Skip(1))
                {
                    var aGroup = SplitForOperator(eof.BinaryOperator, result.ExpressionOrFormulas);
                    var bGroup = SplitForOperator(eof.BinaryOperator, eof is Formula<T> bFormula ? bFormula.ExpressionOrFormulas : new List<ExpressionOrFormula<T>> { eof });
                    var iteration = new List<ExpressionOrFormula<T>>();
                    foreach(var a in aGroup)
                    {
                        foreach(var b in bGroup)
                        {
                            var aFirst = a.First();
                            var bFirst = b.First();
                            iteration.Add(aFirst.WithOperators(bFirst.BinaryOperator, aFirst.UnaryOperators));
                        }
                    }
                }
            }

            return expressionOrFormula;
        }

        public static bool IsFlat<T>(this ExpressionOrFormula<T> expressionOrFormula)
        {
            return expressionOrFormula is not Formula<T> formula
                || formula.ExpressionOrFormulas.Count() > 1 
                && formula.ExpressionOrFormulas.All(eof => eof is not Formula<T>);
        }

        private static IEnumerable<IEnumerable<ExpressionOrFormula<T>>> SplitForOperator<T>(BinaryOperator binaryOperator, IEnumerable<ExpressionOrFormula<T>> expressionOrFormulas)
        {
            var remainingEofs = expressionOrFormulas;
            while (remainingEofs.Any())
            {
                var group = remainingEofs.TakeWhile(_ => (_.BinaryOperator?.CompareTo(binaryOperator) ?? -1) <= 0);
                remainingEofs = remainingEofs.Skip(group.Count());
                yield return group;
            }
        }
    }
}
