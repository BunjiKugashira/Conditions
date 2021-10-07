namespace PropositionalCalculus.BinaryOperators
{
    using System;

    public class InvalidOperator : BinaryOperator
    {
        public InvalidOperator()
        {
            throw new InvalidOperationException("No suitable operator found.");
        }

        public override int CompareTo(BinaryOperator other)
        {
            throw new InvalidOperationException("This exception should be impossible to reach.");
        }

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            throw new InvalidOperationException("This exception should be impossible to reach.");
        }

        public override bool Resolve(bool a, bool b)
        {
            throw new InvalidOperationException("This exception should be impossible to reach.");
        }
    }
}
