namespace PropositionalCalculus.Tests
{
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
            a = a.WithOperators(And.Instance, new List<UnaryOperator>() { Not.Instance });

            Assert.Equal("& !a", a.ToString());
        }

        [Fact]
        public void TestWherePredicate()
        {
            var a = new Expression<string>("a");

            var result = a.Where(value => value == "a");
            Assert.NotNull(result);

            result = a.Where(value => value == "b");
            Assert.Null(result);
        }

        [Fact]
        public void TestWhereType()
        {
            var a = new Expression<object>("a");

            var resultString = a.Where<string>();
            Assert.NotNull(resultString);

            var resultInt = a.Where<int>();
            Assert.Null(resultInt);
        }
    }
}
