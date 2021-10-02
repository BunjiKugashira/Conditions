namespace PropositionalCalculus.BinaryOperators
{
    using System.Collections.Generic;

    using UnaryOperators;

    public class Nor : BinaryOperator
    {
        public override BinaryOperator CounterOperator => throw new System.NotImplementedException();

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            return new Formula<T>(a.BinaryOperator, new List<UnaryOperator>() { UnaryOperator.NOT }, a.WithOperators(null, a.UnaryOperators), b.WithOperators(OR, b.UnaryOperators));
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
