namespace CollectionsExtensions.Test
{
    using Xunit;

    public class LambdaEqualityComparerTest
    {
        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 0)]
        [InlineData(true, 1)]
        [InlineData(false, 1)]
        [InlineData(true, 100)]
        [InlineData(false, 100)]
        public void EqualsAndHashCode_WithPredefinedResults_ShouldMatchResults(bool shouldBeEqual, int shouldHaveHashCode)
        {
            // Arrange
            var comparer = new LambdaEqualityComparer<object>((_, _) => shouldBeEqual, _ => shouldHaveHashCode);

            // Act
            var isEqual = comparer.Equals(new object(), new object());
            var hash = comparer.GetHashCode(new object());

            // Assert
            Assert.Equal(shouldBeEqual, isEqual);
            Assert.Equal(shouldHaveHashCode, hash);
        }

        [Fact]
        public void EqualsAndHashCode_WithObjects_ShouldPassParameters()
        {
            // Arrange
            var obA = new object();
            var obB = new object();
            var obHash = new object();

            var passedObA = false;
            var passedObB = false;
            var passedObHash = false;

            var comparer = new LambdaEqualityComparer<object>((object a, object b) =>
            {
                passedObA = a == obA;
                passedObB = b == obB;
                return true;
            }, (object hash) =>
            {
                passedObHash = hash == obHash;
                return 0;
            });

            // Act
            comparer.Equals(obA, obB);
            comparer.GetHashCode(obHash);

            // Assert
            Assert.True(passedObA);
            Assert.True(passedObB);
            Assert.True(passedObHash);
        }
    }
}
