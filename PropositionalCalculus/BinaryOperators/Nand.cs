namespace PropositionalCalculus.BinaryOperators
{
    using System;

    public sealed class Nand : BinaryOperator
    {
        private static readonly Lazy<Nand> lazy = new(() => new Nand());
        public static Nand Instance { get => lazy.Value; }

        private Nand()
        {
        }

        public override int CompareTo(BinaryOperator other)
        {
            return other switch
            {
                And or Nand => 0,
                Or or Nor => -1,
                Xor or Nxor => -1,
                _ => -other.CompareTo(this),
            };
        }

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            if (b.BinaryOperator is not Nand)
            {
                throw new ArgumentException("Operator must be of type " + nameof(Nand));
            }

            return !(a & b);
        }

        public override bool Resolve(bool a, bool b)
        {
            return !(a && b);
        }

        public override string ToString()
        {
            return "!&";
        }
    }
}
