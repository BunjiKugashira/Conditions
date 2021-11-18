namespace CollectionsExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EnumerableSequenceEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        private readonly IEqualityComparer<T> itemComparer;

        public EnumerableSequenceEqualityComparer(IEqualityComparer<T> itemComparer = null)
        {
            this.itemComparer = itemComparer;
        }

        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            return x == null || y == null
                ? x == null && y == null
                : x.SequenceEqual(y, this.itemComparer);
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            return obj?.Aggregate(0, (aggr, item) => HashCode.Combine(aggr, this.itemComparer?.GetHashCode(item) ?? item?.GetHashCode() ?? 0)) ?? 0;
        }
    }
}
