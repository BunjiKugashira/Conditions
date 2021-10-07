namespace Conditions.Tests
{
    using System;
    using System.Collections.Generic;

    using PropositionalCalculus;
    using PropositionalCalculus.BinaryOperators;
    using PropositionalCalculus.UnaryOperators;

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

            b = new Expression<string>("b");
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void TestHashCode()
        {
            var a = new Expression<string>("a");
            var b = new Expression<string>("a");
            Assert.Equal(a.GetHashCode(), b.GetHashCode());

            b = new Expression<string>("b");
            Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
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

        [Fact]
        public void TestWithOperator()
        {
            var a = new Expression<string>("a");
            a = a.WithOperators(BinaryOperator.AND, new List<UnaryOperator>() { UnaryOperator.NOT });

            Assert.Equal("& !a", a.ToString());
        }
    }
}
