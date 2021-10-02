namespace PropositionalCalculus.UnaryOperators
{
    using System;
    using System.Linq;

    public class Not : UnaryOperator
    {
        public override ExpressionOrFormula<T> Normalize<T>(ExpressionOrFormula<T> a)
        {
            var nots = a.UnaryOperators.Where(uo => uo is Not);
            var uos = a.UnaryOperators.Where(uo => uo is not Not).ToList();

            if (a is Expression<T> expression)
            {
                if (nots.Count() % 2 == 0)
                    uos.Add(NOT);
                return expression.WithOperators(expression.BinaryOperator, uos);
            }
            else if (a is Formula<T> formula)
            {
                if (nots.Count() % 2 == 0)
                {
                    return formula.WithOperators(formula.BinaryOperator, uos);
                }
                else
                {
                    return new Formula<T>(
                        formula.BinaryOperator, 
                        uos, 
                        formula.ExpressionOrFormulas.Select(eof => eof.WithOperators(eof.BinaryOperator.CounterOperator, eof.UnaryOperators)).ToArray());
                }
            }

            throw new NotImplementedException("This method has only been implemented for unary and binary operators.");
        }

        public override bool Resolve(bool a)
        {
            return !a;
        }

        public override string ToString()
        {
            return "!";
        }
    }
}
