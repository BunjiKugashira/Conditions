namespace PropositionalCalculus.BinaryOperators
{
    using System;

    public sealed class Or : BinaryOperator
    {
        private static readonly Lazy<Or> lazy = new(() => new Or());
        public static Or Instance { get => lazy.Value; }

        private Or()
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
            return new Formula<T>(a.BinaryOperator, a.WithOperators(null, a.UnaryOperators), b);
        }

        public override bool Resolve(bool a, bool b)
        {
            return a || b;
        }

        public override string ToString()
        {
            return "|";
        }
    }
}
