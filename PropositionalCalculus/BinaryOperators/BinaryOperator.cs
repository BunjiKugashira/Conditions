namespace PropositionalCalculus.BinaryOperators
{
    using System;

    public abstract class BinaryOperator : IComparable<BinaryOperator>
    {
        public abstract bool Resolve(bool a, bool b);

        public abstract ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b);

        public abstract int CompareTo(BinaryOperator other);
    }
}
