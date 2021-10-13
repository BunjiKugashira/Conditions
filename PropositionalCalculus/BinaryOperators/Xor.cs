namespace PropositionalCalculus.BinaryOperators
{
    using System;

    public sealed class Xor : BinaryOperator
    {
        private static readonly Lazy<Xor> lazy = new(() => new Xor());
        public static Xor Instance { get => lazy.Value; }

        private Xor()
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
            if (b.BinaryOperator is not Xor)
            {
                throw new ArgumentException("Operator must be of type " + nameof(Xor));
            }

            return a & !b | !a & b;
        }

        public override bool Resolve(bool a, bool b)
        {
            return a ^ b;
        }

        public override string ToString()
        {
            return "^";
        }
    }
}
