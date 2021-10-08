namespace PropositionalCalculus.BinaryOperators
{
    using System;

    public sealed class Nor : BinaryOperator
    {
        private static readonly Lazy<Nor> lazy = new(() => new Nor());
        public static Nor Instance { get => lazy.Value; }

        private Nor()
        {
        }

        public override int CompareTo(BinaryOperator other)
        {
            return other switch
            {
                And or Nand => 1,
                Or or Nor => 0,
                Xor or Nxor => 1,
                _ => -other.CompareTo(this),
            };
        }

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            return !(a | b);
        }

        public override bool Resolve(bool a, bool b)
        {
            return !(a || b);
        }

        public override string ToString()
        {
            return "!|";
        }
    }
}
