namespace PropositionalCalculus.BinaryOperators
{
    public class Or : BinaryOperator
    {
        public override BinaryOperator CounterOperator => AND;

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
