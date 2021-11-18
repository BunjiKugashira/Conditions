namespace CollectionsExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        protected readonly Func<T, T, bool> equalityLambda;
        protected readonly Func<T, int> hashFunction;

        public LambdaEqualityComparer([DisallowNull] Func<T, T, bool> comparisonFunction, [DisallowNull] Func<T, int> hashFunction)
        {
            this.equalityLambda = comparisonFunction;
            this.hashFunction = hashFunction;
        }

        public bool Equals(T x, T y)
        {
            return x == null || y == null
                ? x == null && y == null
                : this.equalityLambda(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj == null
                ? 0
                : this.hashFunction(obj);
        }
    }
}
