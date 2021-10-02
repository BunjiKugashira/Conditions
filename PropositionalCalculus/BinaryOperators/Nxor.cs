namespace PropositionalCalculus.BinaryOperators
{
    public class Nxor : BinaryOperator
    {
        public override BinaryOperator CounterOperator => throw new System.NotImplementedException();

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            throw new System.NotImplementedException();
        }

        public override bool Resolve(bool a, bool b)
        {
            return !(a ^ b);
        }

        public override string ToString()
        {
            return "!^";
        }
    }
}
