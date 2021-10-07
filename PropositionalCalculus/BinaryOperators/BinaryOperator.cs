namespace PropositionalCalculus.BinaryOperators
{
    using System;

    public abstract class BinaryOperator : IComparable<BinaryOperator>
    {
        public static InvalidOperator INVALID_OPERATOR { get => new InvalidOperator(); }
        public static And AND { get; } = new And();
        public static Or OR { get; } = new Or();
        public static Xor XOR { get; } = new Xor();
        public static Nand NAND { get; } = new Nand();
        public static Nor NOR { get; } = new Nor();
        public static Nxor NXOR { get; } = new Nxor();

        public abstract bool Resolve(bool a, bool b);

        public abstract ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b);

        public abstract int CompareTo(BinaryOperator other);
    }
}
