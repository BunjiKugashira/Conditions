namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PropositionalCalculus.UnaryOperators;

    public class Formula<T> : ExpressionOrFormula<T>
    {
        public IEnumerable<ExpressionOrFormula<T>> ExpressionOrFormulas { get; }

        public Formula(BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperator> unaryOperators, params ExpressionOrFormula<T>[] expressionOrFormulas) 
            : base(binaryOperator, unaryOperators)
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(IEnumerable<UnaryOperator> unaryOperators, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(null, unaryOperators)
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(BinaryOperators.BinaryOperator binaryOperator, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(binaryOperator, null)
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(null, null)
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        private static void ValidateExpressionOrFormulas(IEnumerable<ExpressionOrFormula<T>> expressionOrFormulas)
        {
            if (expressionOrFormulas.Count() > 0)
            {
                if (expressionOrFormulas.First().BinaryOperator != null)
                {
                    throw new ArgumentException($@"First expression or formula must not have a binary operator. Instead found Operator {expressionOrFormulas.First().BinaryOperator}.");
                }

                foreach (var expressionOrFormula in expressionOrFormulas.Skip(1))
                {
                    if (expressionOrFormula.BinaryOperator == null)
                    {
                        throw new ArgumentNullException(nameof(expressionOrFormula.BinaryOperator), $@"All expressions or formulas except the first one must have a binary operator.");
                    }
                }
            }
        }

        public override Formula<T> WithOperators(BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperator> unaryOperators)
        {
            return new Formula<T>(binaryOperator, unaryOperators, this.ExpressionOrFormulas.ToArray());
        }

        public override string ToString()
        {
            return base.ToString() + this.SubExpressionOrFormulasToString();
        }

        private string ToParenthesisString()
        {
            return base.ToString() + "(" + this.SubExpressionOrFormulasToString() + ")";
        }

        private string SubExpressionOrFormulasToString()
        {
            return string.Join(" ", this.ExpressionOrFormulas.Select(eof => eof is Formula<T> f ? f.ToParenthesisString() : eof.ToString()));
        }

        public override bool Equals(object obj)
        {
            return obj is Formula<T> formula
                    && base.Equals(formula)
                    && this.ExpressionOrFormulas.SequenceEqual(formula.ExpressionOrFormulas);
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();

            foreach(var expressionOrFormula in this.ExpressionOrFormulas)
            {
                hashCode = HashCode.Combine(hashCode, expressionOrFormula.GetHashCode());
            }

            return hashCode;
        }

        public static Formula<T> operator !(Formula<T> a) => a.WithOperators(a.BinaryOperator, a.UnaryOperators.Prepend(UnaryOperator.NOT));
    }
}
