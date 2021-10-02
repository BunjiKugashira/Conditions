namespace PropositionalCalculus.UnaryOperators
{
    using System;

    public class InvalidOperator : UnaryOperator
    {
        public InvalidOperator()
        {
            throw new InvalidOperationException("No suitable operator found.");
        }

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a)
        {
            throw new InvalidOperationException("This exception should be impossible to reach.");
        }

        public override bool Resolve(bool a)
        {
            throw new InvalidOperationException("This exception should be impossible to reach.");
        }
    }
}
