namespace PropositionalCalculus.UnaryOperators
{
    using System;

    public abstract class UnaryOperator : IComparable<UnaryOperator>
    {
        public static InvalidOperator INVALID_OPERATOR { get => new InvalidOperator(); }
        public static Not NOT { get; } = new Not();

        public abstract bool Resolve(bool a);

        public abstract ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a);

        public abstract int CompareTo(UnaryOperator other);
    }
}
