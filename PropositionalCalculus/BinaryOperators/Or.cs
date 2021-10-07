namespace PropositionalCalculus.BinaryOperators
{
    public class Or : BinaryOperator
    {
        public override int CompareTo(BinaryOperator other)
        {
            switch (other)
            {
                case And:
                case Nand:
                    return 1;
                case Or:
                case Nor:
                    return 0;
                case Xor:
                case Nxor:
                    return 1;
                default:
                    return -other.CompareTo(this);
            }
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
