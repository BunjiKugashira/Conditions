namespace CollectionsExtensions.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public class EnumerableSequenceEqualityComparerTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        public void EqualsAndHashCode_WithoutItemComparer_ShouldBeEqual(int length)
        {
            // Arrange
            var listA = this.GenerateEnumerable(length);
            var listB = this.GenerateEnumerable(length);
            var comparer = new EnumerableSequenceEqualityComparer<string>();

            // Act
            var isEqual = comparer.Equals(listA, listB);
            var hashA = comparer.GetHashCode(listA);
            var hashB = comparer.GetHashCode(listB);

            // Assert
            Assert.True(isEqual);
            Assert.Equal(hashA, hashB);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(2, 1)]
        [InlineData(10, 0)]
        [InlineData(10, 5)]
        [InlineData(10, 9)]
        [InlineData(100, 0)]
        [InlineData(100, 50)]
        [InlineData(100, 99)]
        public void EqualsAndHashCode_WithoutItemComparer_ShouldNotBeEqual(int length, int diffIndex)
        {
            // Arrange
            var listA = this.GenerateEnumerable(length);
            var listB = this.GenerateEnumerable(length, listA, diffIndex);
            var comparer = new EnumerableSequenceEqualityComparer<string>();

            // Act
            var isEqual = comparer.Equals(listA, listB);
            var hashA = comparer.GetHashCode(listA);
            var hashB = comparer.GetHashCode(listB);

            // Assert
            Assert.False(isEqual);
            Assert.NotEqual(hashA, hashB);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(2, 1)]
        [InlineData(10, 0)]
        [InlineData(10, 5)]
        [InlineData(10, 9)]
        [InlineData(100, 0)]
        [InlineData(100, 50)]
        [InlineData(100, 99)]
        public void EqualsAndHashCode_WithItemComparer_ShouldBeEqual(int length, int diffIndex)
        {
            // Arrange
            var listA = this.GenerateEnumerable(length);
            var listB = this.GenerateEnumerable(length, listA, diffIndex);
            var itemComparer = new LambdaEqualityComparer<string>((string a, string b) => a.Last().Equals(b.Last()), a => a.Last().GetHashCode());
            var comparer = new EnumerableSequenceEqualityComparer<string>(itemComparer);

            // Act
            var isEqual = comparer.Equals(listA, listB);
            var hashA = comparer.GetHashCode(listA);
            var hashB = comparer.GetHashCode(listB);

            // Assert
            Assert.True(isEqual);
            Assert.Equal(hashA, hashB);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(2, 1)]
        [InlineData(10, 0)]
        [InlineData(10, 5)]
        [InlineData(10, 9)]
        [InlineData(100, 0)]
        [InlineData(100, 50)]
        [InlineData(100, 99)]
        public void EqualsAndHashCode_WithItemComparer_ShouldNotBeEqual(int length, int diffIndex)
        {
            // Arrange
            var listA = this.GenerateEnumerable(length);
            var listB = this.GenerateEnumerable(length, listA, diffIndex);
            var itemComparer = new LambdaEqualityComparer<string>((string a, string b) => a.First().Equals(b.First()), a => a.First().GetHashCode());
            var comparer = new EnumerableSequenceEqualityComparer<string>(itemComparer);

            // Act
            var isEqual = comparer.Equals(listA, listB);
            var hashA = comparer.GetHashCode(listA);
            var hashB = comparer.GetHashCode(listB);

            // Assert
            Assert.False(isEqual);
            Assert.NotEqual(hashA, hashB);
        }

        private IEnumerable<string> GenerateEnumerable(int length, IEnumerable<string> template = null, int? diffIndex = null)
        {
            var enumerable = new string[length];
            for (var i = 0; i < enumerable.Length; i++)
            {
                enumerable[i] = diffIndex != i ? template?.ElementAtOrDefault(i) ?? $"value{i}" : $"diff{i}";
            }

            return enumerable;
        }
    }
}
