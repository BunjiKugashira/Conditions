namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PropositionalCalculus.UnaryOperators;

    public class Formula<T> : ExpressionOrFormula<T>
    {
        public IEnumerable<ExpressionOrFormula<T>> ExpressionOrFormulas { get; }

        public bool HasBrackets { get; }

        public Formula(BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperators.UnaryOperator> unaryOperators, params ExpressionOrFormula<T>[] expressionOrFormulas) 
            : base(binaryOperator, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperators.UnaryOperator>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(IEnumerable<UnaryOperators.UnaryOperator> unaryOperators, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(null, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperators.UnaryOperator>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(BinaryOperators.BinaryOperator binaryOperator, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(binaryOperator, Array.Empty<UnaryOperators.UnaryOperator>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(null, Array.Empty<UnaryOperators.UnaryOperator>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(bool hasBrackets, BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperators.UnaryOperator> unaryOperators, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(binaryOperator, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperators.UnaryOperator>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.HasBrackets = hasBrackets;
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(bool hasBrackets, IEnumerable<UnaryOperators.UnaryOperator> unaryOperators, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(null, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperators.UnaryOperator>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.HasBrackets = hasBrackets;
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(bool hasBrackets, BinaryOperators.BinaryOperator binaryOperator, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(binaryOperator, Array.Empty<UnaryOperators.UnaryOperator>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.HasBrackets = hasBrackets;
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(bool hasBrackets, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(null, Array.Empty<UnaryOperators.UnaryOperator>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.HasBrackets = hasBrackets;
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        private Formula(BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperators.UnaryOperator> unaryOperators, Formula<T> template)
            : base(binaryOperator, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperators.UnaryOperator>())
        {
            this.HasBrackets = template.HasBrackets;
            this.ExpressionOrFormulas = template.ExpressionOrFormulas;
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

        public override Formula<T> WithOperators(BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperators.UnaryOperator> unaryOperators)
        {
            return new Formula<T>(binaryOperator, unaryOperators, this);
        }

        public override string ToString()
        {
            return this.HasBrackets ? $@"{base.ToString()}({string.Join(" ", this.ExpressionOrFormulas)})" : $@"{base.ToString()}{string.Join(" ", this.ExpressionOrFormulas)}";
        }

        public override bool Equals(object obj)
        {
            return obj is Formula<T> formula
                    && base.Equals(formula)
                    && this.HasBrackets.Equals(formula.HasBrackets)
                    && this.ExpressionOrFormulas.SequenceEqual(formula.ExpressionOrFormulas);
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();

            foreach(var expressionOrFormula in this.ExpressionOrFormulas)
            {
                hashCode = HashCode.Combine(hashCode, expressionOrFormula.GetHashCode());
            }

            return HashCode.Combine(hashCode, HasBrackets.GetHashCode());
        }

        public static Formula<T> operator !(Formula<T> a) => a.WithOperators(a.BinaryOperator, a.UnaryOperators.Append(UnaryOperator.NOT));
    }
}
