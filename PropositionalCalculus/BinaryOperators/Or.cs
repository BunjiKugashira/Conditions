namespace PropositionalCalculus.BinaryOperators
{
    using System;
using System.Collections.Generic;

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
            if (b.BinaryOperator is not Or)
            {
                throw new ArgumentException("Operator must be of type " + nameof(Or));
            }

            var groupA = a is Formula<T> listA
                ? listA.ExpressionOrFormulas
                : new List<ExpressionOrFormula<T>>() { a, };

            var groupB = b is Formula<T> listB
                ? listB.ExpressionOrFormulas
                : new List<ExpressionOrFormula<T>>() { b, };

            var ret = new Formula<T>();

            foreach (var subA in groupA)
            {
                foreach (var subB in groupB)
                {
                    ret = ExpressionOrFormula<T>.CombineWithOperator(ret, subA.BinaryOperator, subA | subB);
                }
            }

            return ret;
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
