namespace PropositionalCalculus.UnaryOperators
{
    using System;
    using System.Linq;

    using PropositionalCalculus.BinaryOperators;

    public sealed class Not : UnaryOperator
    {
        private static readonly Lazy<Not> lazy = new(() => new Not());
        public static Not Instance { get => lazy.Value; }

        private Not()
        {
        }

        public override int CompareTo(UnaryOperator other)
        {
            return other switch
            {
                Not => 0,
                _ => -other.CompareTo(this),
            };
        }

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a)
        {
            var nots = a.UnaryOperators.Where(uo => uo is Not);
            var uos = a.UnaryOperators.Where(uo => uo is not Not).ToList();

            if (a is Expression<T> expression)
            {
                if (nots.Count() % 2 == 0)
                    uos.Add(Not.Instance);
                return expression.WithOperators(expression.BinaryOperator, uos);
            }
            else if (a is Formula<T> formula)
            {
                return nots.Count() % 2 == 0
                    ? formula.WithOperators(formula.BinaryOperator, uos)
                    : new Formula<T>(
                        formula.BinaryOperator,
                        uos,
                        formula.ExpressionOrFormulas.Select(eof =>
                        {
                            return eof.BinaryOperator switch
                            {
                                null => eof.WithOperators(null, eof.UnaryOperators),
                                And => eof.WithOperators(Or.Instance, eof.UnaryOperators),
                                Or => eof.WithOperators(And.Instance, eof.UnaryOperators),
                                _ => throw new NotImplementedException($@"Unary operator ""Not"" is not implemented for binary operator ""{eof.BinaryOperator?.GetType()}"""),
                            };
                        }).ToArray());
            }

            throw new NotImplementedException("This method has only been implemented for unary and binary operators.");
        }

        public override bool Resolve(bool a)
        {
            return !a;
        }

        public override string ToString()
        {
            return "!";
        }
    }
}
