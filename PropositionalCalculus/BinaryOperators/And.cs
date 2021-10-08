namespace PropositionalCalculus.BinaryOperators
{
    using System;

    public sealed class And : BinaryOperator
    {
        private static readonly Lazy<And> lazy = new(() => new And());
        public static And Instance { get => lazy.Value; }

        private And()
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
            return new Formula<T>(a.BinaryOperator, a.WithOperators(null, a.UnaryOperators), b);
        }

        public override bool Resolve(bool a, bool b)
        {
            return a && b;
        }

        public override string ToString()
        {
            return "&";
        }
    }
}
