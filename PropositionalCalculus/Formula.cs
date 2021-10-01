namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Formula<T> : ExpressionOrFormula<T>
    {
        public IEnumerable<ExpressionOrFormula<T>> ExpressionOrFormulas { get; }

        public Formula(BinaryOperatorEnum? binaryOperator, IEnumerable<UnaryOperatorEnum> unaryOperators, params ExpressionOrFormula<T>[] expressionOrFormulas) 
            : base(binaryOperator, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperatorEnum>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(IEnumerable<UnaryOperatorEnum> unaryOperators, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(null, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperatorEnum>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(BinaryOperatorEnum? binaryOperator, params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(binaryOperator, Array.Empty<UnaryOperatorEnum>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        public Formula(params ExpressionOrFormula<T>[] expressionOrFormulas)
            : base(null, Array.Empty<UnaryOperatorEnum>())
        {
            ValidateExpressionOrFormulas(expressionOrFormulas);
            this.ExpressionOrFormulas = expressionOrFormulas ?? Enumerable.Empty<ExpressionOrFormula<T>>();
        }

        private Formula(BinaryOperatorEnum? binaryOperator, IEnumerable<UnaryOperatorEnum> unaryOperators, Formula<T> template)
            : base(binaryOperator, unaryOperators?.ToArray() ?? Array.Empty<UnaryOperatorEnum>())
        {
            this.ExpressionOrFormulas = template.ExpressionOrFormulas;
        }

        private static void ValidateExpressionOrFormulas(IEnumerable<ExpressionOrFormula<T>> expressionOrFormulas)
        {
            if (expressionOrFormulas.Count() > 0)
            {
                if (expressionOrFormulas.First().BinaryOperator != null)
                {
                    throw new ArgumentException($@"First expression or formula must not have a binary operator. Instead found Operator {expressionOrFormulas.First().BinaryOperator}.");
                }

                foreach (var expressionOrFormula in expressionOrFormulas.Skip(1))
                {
                    if (expressionOrFormula.BinaryOperator == null)
                    {
                        throw new ArgumentNullException($@"All expressions or formulas except the first one must have a binary operator.");
                    }
                }
            }
        }

        public override ExpressionOrFormula<T> WithOperators(BinaryOperatorEnum binaryOperator, IEnumerable<UnaryOperatorEnum> unaryOperators)
        {
            return new Formula<T>(binaryOperator, unaryOperators, this);
        }

        public static Formula<T> CombineWithOperator(Formula<T> a, BinaryOperatorEnum o, Formula<T> b)
        {
            var aeofs = a.ExpressionOrFormulas.ToList();
            var beofs = b.ExpressionOrFormulas.ToArray();

            beofs[0] = beofs[0].WithOperators(o, beofs[0].UnaryOperators);
            aeofs.AddRange(beofs);

            return new Formula<T>(aeofs.ToArray());
        }

        public static Formula<T> CombineWithOperator(Formula<T> a, BinaryOperatorEnum o, Expression<T> b)
        {
            var aeofs = a.ExpressionOrFormulas.ToList();
            var beof = b.WithOperators(o, b.UnaryOperators);

            aeofs.Add(beof);

            return new Formula<T>(aeofs.ToArray());
        }

        public static Formula<T> CombineWithOperator(Expression<T> a, BinaryOperatorEnum o, Formula<T> b)
        {
            var aeof = a.WithOperators(o, a.UnaryOperators);
            var beofs = b.ExpressionOrFormulas.ToList();

            beofs.Insert(0, aeof);

            return new Formula<T>(beofs.ToArray());
        }

        public static Formula<T> AddOperator(UnaryOperatorEnum o, Formula<T> a)
        {
            var operators = a.UnaryOperators.ToList();
            
            operators.Insert(0, o);

            return new Formula<T>(a.BinaryOperator, operators, a);
        }

        public static Formula<T> operator &(Formula<T> a, Formula<T> b) => CombineWithOperator(a, BinaryOperatorEnum.AND, b);
        public static Formula<T> operator &(Formula<T> a, Expression<T> b) => CombineWithOperator(a, BinaryOperatorEnum.AND, b);
        public static Formula<T> operator &(Expression<T> a, Formula<T> b) => CombineWithOperator(a, BinaryOperatorEnum.AND, b);

        public static Formula<T> operator |(Formula<T> a, Formula<T> b) => CombineWithOperator(a, BinaryOperatorEnum.OR, b);
        public static Formula<T> operator |(Formula<T> a, Expression<T> b) => CombineWithOperator(a, BinaryOperatorEnum.OR, b);
        public static Formula<T> operator |(Expression<T> a, Formula<T> b) => CombineWithOperator(a, BinaryOperatorEnum.OR, b);

        public static Formula<T> operator ^(Formula<T> a, Formula<T> b) => CombineWithOperator(a, BinaryOperatorEnum.XOR, b);
        public static Formula<T> operator ^(Formula<T> a, Expression<T> b) => CombineWithOperator(a, BinaryOperatorEnum.XOR, b);
        public static Formula<T> operator ^(Expression<T> a, Formula<T> b) => CombineWithOperator(a, BinaryOperatorEnum.XOR, b);

        public static Formula<T> operator !(Formula<T> a) => AddOperator(UnaryOperatorEnum.NOT, a);
    }
}
