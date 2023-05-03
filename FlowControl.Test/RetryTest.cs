namespace FlowControl.Test
{
    using System;

    using Xunit;
    using FlowControl;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RetryTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task AlwaysThrows_WithLimitedNumberOfTries_ShouldRepeatGivenAmountOfTimesAsync(uint numberOfTries)
        {
            // Arrange
            var tryCounter = 0u;
            var retry = new Retry
            {
                MaxTryNr = numberOfTries - 1,
                CatchDuringCallbacks = new List<Tuple<Type, Action<Retry, Exception>>>
                {
                    new Tuple<Type, Action<Retry, Exception>>(typeof(Exception), (_, _) => { tryCounter++; }),
                },
                CatchAfterCallbacks = new List<Tuple<Type, Action<Retry, Exception>>>
                {
                    new Tuple<Type, Action<Retry, Exception>>(typeof(Exception), (_, _) => { }),
                },
            };

            // Act
            await retry.StartAsync(() => throw new Exception());

            // Assert
            Assert.Equal(numberOfTries, tryCounter);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task AlwaysThrows_WithCatchAfter_ShouldNotThrow(uint numberOfTries)
        {
            // Arrange
            var retry = new Retry
            {
                MaxTryNr = numberOfTries - 1,
                CatchAfterCallbacks = new List<Tuple<Type, Action<Retry, Exception>>>
                {
                    new Tuple<Type, Action<Retry, Exception>>(typeof(Exception), (_, _) => { })
                },
            };

            // Act
            var func = retry.StartAsync(() => throw new Exception());

            // Assert
            await func; // Assert does not throw
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task AlwaysThrows_WithoutCatchAfter_ShouldThrow(uint numberOfTries)
        {
            // Arrange
            var retry = new Retry
            {
                MaxTryNr = numberOfTries - 1,
                CatchDuringCallbacks = new List<Tuple<Type, Action<Retry, Exception>>>
                {
                    new Tuple<Type, Action<Retry, Exception>>(typeof(Exception), (_, _) => { })
                },
            };

            // Act
            var func = retry.StartAsync(() => throw new Exception());

            // Assert
            await Assert.ThrowsAsync<AggregateException>(async () => await func);
        }

        [Fact]
        public void NeedsMoreTests()
        {
            throw new NotImplementedException();
        }
    }
}
