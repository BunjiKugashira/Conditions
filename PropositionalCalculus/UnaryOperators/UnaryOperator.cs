namespace PropositionalCalculus.UnaryOperators
{
    public abstract class UnaryOperator
    {
        public static InvalidOperator INVALID_OPERATOR { get => new InvalidOperator(); }
        public static Not NOT { get; } = new Not();

        public abstract bool Resolve(bool a);

        public abstract ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a);
    }
}
