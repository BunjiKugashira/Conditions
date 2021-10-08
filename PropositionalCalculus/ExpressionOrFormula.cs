namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PropositionalCalculus.UnaryOperators;

    public abstract class ExpressionOrFormula<T>
    {
        public IEnumerable<UnaryOperator> UnaryOperators { get; }

        public BinaryOperators.BinaryOperator BinaryOperator { get; }

        public ExpressionOrFormula(BinaryOperators.BinaryOperator binaryOperator, params UnaryOperator[] unaryOperators)
        {
            this.BinaryOperator = binaryOperator;
            this.UnaryOperators = unaryOperators?.ToList().AsReadOnly() ?? Enumerable.Empty<UnaryOperator>();
        }

        public ExpressionOrFormula(BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperator> unaryOperators)
        {
            this.BinaryOperator = binaryOperator;
            this.UnaryOperators = unaryOperators?.ToList().AsReadOnly() ?? Enumerable.Empty<UnaryOperator>();
        }

        public abstract ExpressionOrFormula<T> WithOperators(BinaryOperators.BinaryOperator binaryOperator, IEnumerable<UnaryOperator> unaryOperators);

        public override string ToString()
        {
            return (this.BinaryOperator == null ? string.Empty : $@"{this.BinaryOperator} ") + string.Join(string.Empty, this.UnaryOperators);
        }

        public override bool Equals(object obj)
        {
            return obj is ExpressionOrFormula<T> eof
                && (this.BinaryOperator?.Equals(eof.BinaryOperator) ?? eof.BinaryOperator == null)
                && this.UnaryOperators.SequenceEqual(eof.UnaryOperators);
        }

        public override int GetHashCode()
        {
            var hashCode = this.BinaryOperator?.GetHashCode() ?? 0;

            foreach (var unaryOperator in this.UnaryOperators)
            {
                hashCode = HashCode.Combine(hashCode, unaryOperator.GetHashCode());
            }

            return hashCode;
        }

        public static Formula<T> CombineWithOperator(ExpressionOrFormula<T> a, BinaryOperators.BinaryOperator o, ExpressionOrFormula<T> b)
        {
            var aContent = (a is Formula<T> fa 
                && fa.ExpressionOrFormulas.Count() > 1 
                && fa.ExpressionOrFormulas.All(eof => (eof.BinaryOperator?.CompareTo(o) ?? -1) <= 0))
                ? fa.ExpressionOrFormulas.Select((eof, i) => eof.WithOperators(i == 0 ? null : eof.BinaryOperator, eof.UnaryOperators.Concat(fa.UnaryOperators)))
                : new List<ExpressionOrFormula<T>>() { a.WithOperators(null, a.UnaryOperators) };

            var bContent = (b is Formula<T> fb 
                && fb.ExpressionOrFormulas.Count() > 1 
                && fb.ExpressionOrFormulas.All(eof => (eof.BinaryOperator?.CompareTo(o) ?? -1) < 0))
                ? fb.ExpressionOrFormulas.Select((eof, i) => eof.WithOperators(i == 0 ? o : eof.BinaryOperator, eof.UnaryOperators.Concat(fb.UnaryOperators)))
                : new List<ExpressionOrFormula<T>>() { b.WithOperators(o, b.UnaryOperators) };

            return new Formula<T>(a.BinaryOperator, aContent.Concat(bContent).ToArray());
        }

        public static Formula<T> operator &(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            return CombineWithOperator(a, BinaryOperators.BinaryOperator.AND, b);
        }

        public static Formula<T> operator |(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            return CombineWithOperator(a, BinaryOperators.BinaryOperator.OR, b);
        }

        public static Formula<T> operator ^(ExpressionOrFormula<T> a, ExpressionOrFormula<T> b)
        {
            return CombineWithOperator(a, BinaryOperators.BinaryOperator.XOR, b);
        }

        public static ExpressionOrFormula<T> operator !(ExpressionOrFormula<T> a)
        {
            return a.WithOperators(a.BinaryOperator, a.UnaryOperators.Prepend(UnaryOperator.NOT));
        }
    }
}
