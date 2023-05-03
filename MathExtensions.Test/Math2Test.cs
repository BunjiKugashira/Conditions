namespace MathExtensions.Test
{
    using System;

    using Xunit;

    public class Math2Test
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1, 1)]
        [InlineData(1, 4, 1, 5, 2, 6, 3, 7, 4, 5, 9, 6)]
        [InlineData(-9, 7, 4, 3, 8, 7, 9, 7, -7, 2, -4, -9)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)]
        public void MinOrDefault_WithoutLambda_ShouldReturnExpectedValue(int expectedValue, params int[] values)
        {
            // Arrange

            // Act
            var actualValue = Math2.MinOrDefault(values);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1, 1)]
        [InlineData(9, 4, 1, 5, 2, 6, 3, 7, 4, 5, 9, 6)]
        [InlineData(9, 7, 4, 3, 8, 7, 9, 7, -7, 2, -4, -9)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)]
        public void MaxOrDefault_WithoutLambda_ShouldReturnExpectedValue(int expectedValue, params int[] values)
        {
            // Arrange

            // Act
            var actualValue = Math2.MaxOrDefault(values);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1, 1)]
        [InlineData(5, 4, 1, 5, 2, 6, 3, 7, 4, 5, 9, 6)]
        [InlineData(4, 7, 4, 3, 8, 7, 9, 7, -7, 2, -4, -9)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)]
        public void MinOrDefault_WithLambda_ShouldReturnExpectedValue(int expectedValue, params int[] values)
        {
            // Arrange

            // Act
            var actualValue = Math2.MinOrDefault((a, b) => Math.Abs(5 - a) - Math.Abs(5 - b), values);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1, 1)]
        [InlineData(1, 4, 1, 5, 2, 6, 3, 7, 4, 5, 9, 6)]
        [InlineData(-9, 7, 4, 3, 8, 7, 9, 7, -7, 2, -4, -9)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)]
        public void MaxOrDefault_WithLambda_ShouldReturnExpectedValue(int expectedValue, params int[] values)
        {
            // Arrange

            // Act
            var actualValue = Math2.MaxOrDefault((a, b) => Math.Abs(6 - a) - Math.Abs(6 - b), values);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Min_WithoutLambda_ShouldThrow()
        {
            // Arrange
            var values = new int[0];

            // Act
            Action shouldThrow = () => Math2.Min(values);

            // Assert
            Assert.Throws<InvalidOperationException>(shouldThrow);
        }

        [Fact]
        public void Max_WithoutLambda_ShouldThrow()
        {
            // Arrange
            var values = new int[0];

            // Act
            Action shouldThrow = () => Math2.Max(values);

            // Assert
            Assert.Throws<InvalidOperationException>(shouldThrow);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 4, 1, 5, 2, 6, 3, 7, 4, 5, 9, 6)]
        [InlineData(-9, 7, 4, 3, 8, 7, 9, 7, -7, 2, -4, -9)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)]
        public void Min_WithoutLambda_ShouldReturnExpectedValue(int expectedValue, params int[] values)
        {
            // Arrange

            // Act
            var actualValue = Math2.Min(values);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(9, 4, 1, 5, 2, 6, 3, 7, 4, 5, 9, 6)]
        [InlineData(9, 7, 4, 3, 8, 7, 9, 7, -7, 2, -4, -9)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)]
        public void Max_WithoutLambda_ShouldReturnExpectedValue(int expectedValue, params int[] values)
        {
            // Arrange

            // Act
            var actualValue = Math2.Max(values);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(5, 4, 1, 5, 2, 6, 3, 7, 4, 5, 9, 6)]
        [InlineData(4, 7, 4, 3, 8, 7, 9, 7, -7, 2, -4, -9)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)]
        public void Min_WithLambda_ShouldReturnExpectedValue(int expectedValue, params int[] values)
        {
            // Arrange

            // Act
            var actualValue = Math2.Min((a, b) => Math.Abs(5 - a) - Math.Abs(5 - b), values);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 4, 1, 5, 2, 6, 3, 7, 4, 5, 9, 6)]
        [InlineData(-9, 7, 4, 3, 8, 7, 9, 7, -7, 2, -4, -9)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)]
        public void Max_WithLambda_ShouldReturnExpectedValue(int expectedValue, params int[] values)
        {
            // Arrange

            // Act
            var actualValue = Math2.Max((a, b) => Math.Abs(6 - a) - Math.Abs(6 - b), values);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
