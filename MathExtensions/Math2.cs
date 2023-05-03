namespace MathExtensions
{
    using System;
    using System.Linq;

    public static class Math2
    {
        public static TComparable MinOrDefault<TComparable>(Func<TComparable, TComparable, int> comparator, params TComparable[] comparables)
        {
            var min = comparables.FirstOrDefault();
            foreach (var comparable in comparables.Skip(1))
            {
                if (comparator(comparable, min) < 0)
                {
                    min = comparable;
                }
            }

            return min;
        }

        public static TComparable MinOrDefault<TComparable>(params TComparable[] comparables)
            where TComparable : IComparable<TComparable>
        {
            return MinOrDefault((TComparable a, TComparable b) => a.CompareTo(b), comparables);
        }

        public static TComparable Min<TComparable>(Func<TComparable, TComparable, int> comparator, params TComparable[] comparables)
        {
            var min = comparables.First();
            foreach (var comparable in comparables.Skip(1))
            {
                if (comparator(comparable, min) < 0)
                {
                    min = comparable;
                }
            }

            return min;
        }

        public static TComparable Min<TComparable>(params TComparable[] comparables)
            where TComparable : IComparable<TComparable>
        {
            return Min((TComparable a, TComparable b) => a.CompareTo(b), comparables);
        }

        public static TComparable MaxOrDefault<TComparable>(Func<TComparable, TComparable, int> comparator, params TComparable[] comparables)
        {
            var max = comparables.FirstOrDefault();
            foreach (var comparable in comparables.Skip(1))
            {
                if (comparator(comparable, max) > 0)
                {
                    max = comparable;
                }
            }

            return max;
        }

        public static TComparable MaxOrDefault<TComparable>(params TComparable[] comparables)
            where TComparable : IComparable<TComparable>
        {
            return MaxOrDefault((TComparable a, TComparable b) => a.CompareTo(b), comparables);
        }

        public static TComparable Max<TComparable>(Func<TComparable, TComparable, int> comparator, params TComparable[] comparables)
        {
            var max = comparables.First();
            foreach (var comparable in comparables.Skip(1))
            {
                if (comparator(comparable, max) > 0)
                {
                    max = comparable;
                }
            }

            return max;
        }

        public static TComparable Max<TComparable>(params TComparable[] comparables)
            where TComparable : IComparable<TComparable>
        {
            return Max((TComparable a, TComparable b) => a.CompareTo(b), comparables);
        }
    }
}
