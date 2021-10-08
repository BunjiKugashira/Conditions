namespace PropositionalCalculus.UnaryOperators
{
    using System;

    public class InvalidOperator : UnaryOperator
    {
        private static readonly Lazy<InvalidOperator> lazy = new(() => new InvalidOperator());
        public static InvalidOperator Instance { get => lazy.Value; }

        private InvalidOperator()
        {
            throw new InvalidOperationException("No suitable operator found.");
        }

        public override int CompareTo(UnaryOperator other)
        {
            throw new InvalidOperationException("This exception should be impossible to reach.");
        }

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a)
        {
            throw new InvalidOperationException("This exception should be impossible to reach.");
        }

        public override bool Resolve(bool a)
        {
            throw new InvalidOperationException("This exception should be impossible to reach.");
        }
    }
}
