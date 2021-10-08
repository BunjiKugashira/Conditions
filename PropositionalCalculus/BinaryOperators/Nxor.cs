namespace PropositionalCalculus.BinaryOperators
{
    using System;

    public sealed class Nxor : BinaryOperator
    {
        private static readonly Lazy<Nxor> lazy = new(() => new Nxor());
        public static Nxor Instance { get => lazy.Value; }

        private Nxor()
        {
        }

        public override int CompareTo(BinaryOperator other)
        {
            return other switch
            {
                And or Nand => 1,
                Or or Nor => -1,
                Xor or Nxor => 0,
                _ => -other.CompareTo(this),
            };
        }

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            return !(a & !b | !a & b);
        }

        public override bool Resolve(bool a, bool b)
        {
            return !(a ^ b);
        }

        public override string ToString()
        {
            return "!^";
        }
    }
}
