namespace PropositionalCalculus.BinaryOperators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            if (b.BinaryOperator is not And)
            {
                throw new ArgumentException("Operator must be of type " + nameof(And));
            }

            var groupA = a is Formula<T> listA
                ? listA.ExpressionOrFormulas
                : new List<ExpressionOrFormula<T>>() { a, };

            var groupB = b is Formula<T> listB
                ? listB.ExpressionOrFormulas
                : new List<ExpressionOrFormula<T>>() { b, };

            var ret = new Formula<T>();

            foreach(var subA in groupA)
            {
                foreach(var subB in groupB)
                {
                    ret = ExpressionOrFormula<T>.CombineWithOperator(ret, subA.BinaryOperator, subA & subB);
                }
            }

            return ret;
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
