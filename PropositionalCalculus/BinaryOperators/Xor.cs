namespace PropositionalCalculus.BinaryOperators
{
    using System.Linq;

    using PropositionalCalculus.UnaryOperators;

    public class Xor : BinaryOperator
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
                    return -1;
                case Xor:
                case Nxor:
                    return 0;
                default:
                    return -other.CompareTo(this);
            }
        }

        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            return a & !b | !a & b;
        }

        public override bool Resolve(bool a, bool b)
        {
            return a ^ b;
        }

        public override string ToString()
        {
            return "^";
        }
    }
}
