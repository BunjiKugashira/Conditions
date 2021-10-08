namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PropositionalCalculus.UnaryOperators;

    public class Expression<T>
        : ExpressionOrFormula<T>
    {
        public T Value { get; }

        public Expression(BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperator> unaryOperators, T expression)
            : base(binaryOperator, unaryOperators)
        {
            this.Value = expression;
        }

        public Expression(BinaryOperators.BinaryOperator binaryOperator, UnaryOperator unaryOperator, T expression)
            : base(binaryOperator, unaryOperator)
        {
            this.Value = expression;
        }

        public Expression(IEnumerable<UnaryOperator> unaryOperators, T expression)
            : base(null, unaryOperators)
        {
            this.Value = expression;
        }

        public Expression(UnaryOperator unaryOperator, T expression)
            : base(null, unaryOperator)
        {
            this.Value = expression;
        }

        public Expression(BinaryOperators.BinaryOperator binaryOperator, T expression)
            : base(binaryOperator, null)
        {
            this.Value = expression;
        }

        public Expression(T expression)
            : base(null, null)
        {
            this.Value = expression;
        }

        public override Expression<T> WithOperators(BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperator> unaryOperators)
        {
            return new Expression<T>(binaryOperator, unaryOperators, this.Value);
        }

        public override string ToString()
        {
            return base.ToString() + this.Value.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Expression<T> expression
                && base.Equals(expression)
                && this.Value.Equals(expression.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), this.Value.GetHashCode());
        }

        public static Expression<T> operator !(Expression<T> a) => a.WithOperators(a.BinaryOperator, a.UnaryOperators.Prepend(Not.Instance));
    }
}
