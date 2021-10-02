namespace Conditions.Tests
{
    using System;

    using PropositionalCalculus;

    using Xunit;

    public class ExpressionTest
    {
        [Fact]
        public void TestEqualsSelf()
        {
            var a = new Expression<string>("a");
            var b = new Expression<string>("a");

            Assert.Equal(a, a);
            Assert.Equal(a, b);
        }

        [Fact]
        public void TestHashCode()
        {
            var a = new Expression<string>("a");
            var b = new Expression<string>("a");

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void TestToString()
        {
            var a = new Expression<string>("a");

            Assert.Equal("a", a.ToString());
        }

        [Fact]
        public void TestNegation()
        {
            var a = !new Expression<string>("a");

            Assert.Equal("!a", a.ToString());

            a = !a;

            Assert.Equal("!!a", a.ToString());
        }
    }
}
