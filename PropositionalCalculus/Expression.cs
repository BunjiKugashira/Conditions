namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Expression<T>
        : ExpressionOrFormula<T>
    {
        public T Value { get; }

        public Expression(BinaryOperatorEnum? binaryOperator, IEnumerable<UnaryOperatorEnum> unaryOperators, T expression)
            : base(binaryOperator, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperatorEnum>())
        {
            this.Value = expression;
        }

        public Expression(IEnumerable<UnaryOperatorEnum> unaryOperators, T expression)
            : base(null, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperatorEnum>())
        {
            this.Value = expression;
        }

        public Expression(BinaryOperatorEnum? binaryOperator, T expression)
            : base(binaryOperator, Array.Empty<UnaryOperatorEnum>())
        {
            this.Value = expression;
        }

        public Expression(T expression)
            : base(null, Array.Empty<UnaryOperatorEnum>())
        {
            this.Value = expression;
        }

        private Expression(BinaryOperatorEnum? binaryOperator, IEnumerable<UnaryOperatorEnum> unaryOperators, Expression<T> template)
            : base(binaryOperator, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperatorEnum>())
        {
            this.Value = template.Value;
        }

        public override ExpressionOrFormula<T> WithOperators(BinaryOperatorEnum binaryOperator, IEnumerable<UnaryOperatorEnum> unaryOperators)
        {
            return new Expression<T>(binaryOperator, unaryOperators, this);
        }

        public static Expression<T> AddOperator(UnaryOperatorEnum o, Expression<T> a)
        {
            var operators = a.UnaryOperators.ToList();

            operators.Insert(0, o);

            return new Expression<T>(a.BinaryOperator, operators, a);
        }

        public static Expression<T> operator !(Expression<T> a) => AddOperator(UnaryOperatorEnum.NOT, a);
    }
}
