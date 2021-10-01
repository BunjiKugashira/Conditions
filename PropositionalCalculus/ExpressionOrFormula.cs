namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    abstract class ExpressionOrFormula<T>
    {
        public IEnumerable<UnaryOperatorEnum> UnaryOperators { get; }

        public BinaryOperatorEnum? BinaryOperator { get; }

        public ExpressionOrFormula(BinaryOperatorEnum? binaryOperator, params UnaryOperatorEnum[] unaryOperators)
        {
            this.BinaryOperator = binaryOperator;
            this.UnaryOperators = unaryOperators?.ToList().AsReadOnly() ?? Enumerable.Empty<UnaryOperatorEnum>();
        }

        public ExpressionOrFormula(BinaryOperatorEnum? binaryOperator, IEnumerable<UnaryOperatorEnum> unaryOperators, ExpressionOrFormula<T> template)
        {
            this.BinaryOperator = binaryOperator;
            this.UnaryOperators = unaryOperators;
        }

        public abstract ExpressionOrFormula<T> WithOperators(BinaryOperatorEnum binaryOperator, IEnumerable<UnaryOperatorEnum> unaryOperators);
    }
}
