namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PropositionalCalculus.BinaryOperators;
    using PropositionalCalculus.UnaryOperators;

    public class TruthValue<T> : ExpressionOrFormula<T>
    {
        public bool Value { get; }

        public TruthValue(BinaryOperator binaryOperator, IEnumerable<UnaryOperator> unaryOperators, bool value)
            : base(binaryOperator, unaryOperators)
        {
            this.Value = value;
        }

        public TruthValue(IEnumerable<UnaryOperator> unaryOperators, bool value)
            : base(null, unaryOperators)
        {
            this.Value = value;
        }

        public TruthValue(BinaryOperator binaryOperator, bool value)
            : base(binaryOperator, null)
        {
            this.Value = value;
        }

        public TruthValue(bool value)
            : base(null, null)
        {
            this.Value = value;
        }

        public override TruthValue<TNew> Where<TNew>()
        {
            return new TruthValue<TNew>(this.BinaryOperator, this.UnaryOperators, this.Value);
        }

        public override TruthValue<T> Where(Func<T, bool> predicate)
        {
            return this;
        }

        public override TruthValue<T> WithOperators(BinaryOperator binaryOperator, IEnumerable<UnaryOperator> unaryOperators)
        {
            return new TruthValue<T>(binaryOperator, unaryOperators, this.Value);
        }

        public static TruthValue<T> operator !(TruthValue<T> a) => a.WithOperators(a.BinaryOperator, a.UnaryOperators.Prepend(Not.Instance));
    }
}
