namespace PropositionalCalculus.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using PropositionalCalculus;
    using PropositionalCalculus.BinaryOperators;
    using PropositionalCalculus.UnaryOperators;
    using Xunit;

    public class FormulaTest
    {
        [Fact]
        public void TestEqualsSelf()
        {
            var a = new Formula<string>();
            var b = new Formula<string>();
            Assert.Equal(a, a);
            Assert.Equal(a, b);

            a = new Formula<string>(Not.Instance);
            Assert.NotEqual(a, b);

            b = new Formula<string>(Not.Instance);
            Assert.Equal(a, b);

            a = new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b"));
            b = new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b"));
            Assert.Equal(a, a);
            Assert.Equal(a, b);

            b = new Formula<string>(new Expression<string>("a"), new Expression<string>(Or.Instance, "b"));
            Assert.NotEqual(a, b);

            b = new Formula<string>(new Expression<string>("a"), new Expression<string>(Xor.Instance, "b"));
            Assert.NotEqual(a, b);

            a = new Expression<string>("a") & (new Expression<string>("b") & new Expression<string>("c"));
            b = new Expression<string>("a") & (new Expression<string>("b") & new Expression<string>("c"));
            Assert.Equal(a, b);

            b = new Expression<string>("a") & new Expression<string>("b") & new Expression<string>("c");
            Assert.NotEqual(a, b);

            b = (new Expression<string>("a") & new Expression<string>("b")) & new Expression<string>("c");
            Assert.NotEqual(a, b);

            a = (new Expression<string>("a") | new Expression<string>("b")) & new Expression<string>("c");
            b = (new Expression<string>("a") | new Expression<string>("b")) & new Expression<string>("c");
            Assert.Equal(a, b);

            b = new Expression<string>("a") | new Expression<string>("b") & new Expression<string>("c");
            Assert.NotEqual(a, b);

            b = new Expression<string>("a") | (new Expression<string>("b") & new Expression<string>("c"));
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void TestHashCode()
        {
            var a = new Formula<string>();
            var b = new Formula<string>();
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void TestToString()
        {
            var a = new Formula<string>();

            Assert.Equal(string.Empty, a.ToString());
        }

        [Fact]
        public void TestNegation()
        {
            var a = !new Formula<string>();

            Assert.Equal("!", a.ToString());

            a = !a;

            Assert.Equal("!!", a.ToString());
        }

        [Fact]
        public void TestWithOperator()
        {
            var a = new Formula<string>();
            a = a.WithOperators(And.Instance, new List<UnaryOperator>() { Not.Instance });

            Assert.Equal("& !", a.ToString());
        }

        [Fact]
        public void TestAnd()
        {
            var a = new Expression<string>("a");
            var b = new Expression<string>("b");
            var c = new Expression<string>("c");

            var result = a & b & c;
            Assert.Equal("a & b & c", result.ToString());
        }

        [Fact]
        public void TestOr()
        {
            var a = new Expression<string>("a");
            var b = new Expression<string>("b");
            var c = new Expression<string>("c");

            var result = a | b | c;
            Assert.Equal("a | b | c", result.ToString());
        }

        [Fact]
        public void TestXor()
        {
            var a = new Expression<string>("a");
            var b = new Expression<string>("b");
            var c = new Expression<string>("c");

            var result = a ^ b ^ c;
            Assert.Equal("a ^ b ^ c", result.ToString());
        }

        [Fact]
        public void TestMixedOperators()
        {
            var a = new Expression<string>("a");
            var b = new Expression<string>("b");
            var c = new Expression<string>("c");
            var d = new Expression<string>("d");

            var result = a & b | c ^ d;
            Assert.Equal("a & b | c ^ d", result.ToString());
            result = a | b & c ^ d;
            Assert.Equal("a | b & c ^ d", result.ToString());
            result = a | b ^ c & d;
            Assert.Equal("a | b ^ c & d", result.ToString());
            result = a ^ b | c & d;
            Assert.Equal("a ^ b | c & d", result.ToString());
            result = a & b ^ c | d;
            Assert.Equal("a & b ^ c | d", result.ToString());
            result = a ^ b & c | d;
            Assert.Equal("a ^ b & c | d", result.ToString());
        }

        [Fact]
        public void TestSameOperatorsInParenthesis()
        {
            var a = new Expression<string>("a");
            var b = new Expression<string>("b");
            var c = new Expression<string>("c");

            var result = (a & b) & c;
            Assert.Equal("a & b & c", result.ToString());
            result = a & (b & c);
            Assert.Equal("a & (b & c)", result.ToString());
            result = (a | b) | c;
            Assert.Equal("a | b | c", result.ToString());
            result = a | (b | c);
            Assert.Equal("a | (b | c)", result.ToString());
            result = (a ^ b) ^ c;
            Assert.Equal("a ^ b ^ c", result.ToString());
            result = a ^ (b ^ c);
            Assert.Equal("a ^ (b ^ c)", result.ToString());
        }

        [Fact]
        public void TestTwoExpressionsInParenthesis()
        {
            var a = new Expression<string>("a");
            var b = new Expression<string>("b");
            var c = new Expression<string>("c");
            var d = new Expression<string>("d");

            var result = (a & b) | c ^ d;
            Assert.Equal("a & b | c ^ d", result.ToString());
            result = (a | b) & c ^ d;
            Assert.Equal("(a | b) & c ^ d", result.ToString());
            result = (a | b) ^ c & d;
            Assert.Equal("(a | b) ^ c & d", result.ToString());
            result = (a ^ b) | c & d;
            Assert.Equal("a ^ b | c & d", result.ToString());
            result = (a & b) ^ c | d;
            Assert.Equal("a & b ^ c | d", result.ToString());
            result = (a ^ b) & c | d;
            Assert.Equal("(a ^ b) & c | d", result.ToString());

            result = a & (b | c) ^ d;
            Assert.Equal("a & (b | c) ^ d", result.ToString());
            result = a | (b & c) ^ d;
            Assert.Equal("a | b & c ^ d", result.ToString());
            result = a | (b ^ c) & d;
            Assert.Equal("a | (b ^ c) & d", result.ToString());
            result = a ^ (b | c) & d;
            Assert.Equal("a ^ (b | c) & d", result.ToString());
            result = a & (b ^ c) | d;
            Assert.Equal("a & (b ^ c) | d", result.ToString());
            result = a ^ (b & c) | d;
            Assert.Equal("a ^ b & c | d", result.ToString());

            result = a & b | (c ^ d);
            Assert.Equal("a & b | c ^ d", result.ToString());
            result = a | b & (c ^ d);
            Assert.Equal("a | b & (c ^ d)", result.ToString());
            result = a | b ^ (c & d);
            Assert.Equal("a | b ^ c & d", result.ToString());
            result = a ^ b | (c & d);
            Assert.Equal("a ^ b | c & d", result.ToString());
            result = a & b ^ (c | d);
            Assert.Equal("a & b ^ (c | d)", result.ToString());
            result = a ^ b & (c | d);
            Assert.Equal("a ^ b & (c | d)", result.ToString());
        }

        [Fact]
        public void TestThreeExpressionsInParenthesis()
        {

            var a = new Expression<string>("a");
            var b = new Expression<string>("b");
            var c = new Expression<string>("c");
            var d = new Expression<string>("d");

            var result = (a & b | c) ^ d;
            Assert.Equal("(a & b | c) ^ d", result.ToString());
            result = (a | b & c) ^ d;
            Assert.Equal("(a | b & c) ^ d", result.ToString());
            result = (a | b ^ c) & d;
            Assert.Equal("(a | b ^ c) & d", result.ToString());
            result = (a ^ b | c) & d;
            Assert.Equal("(a ^ b | c) & d", result.ToString());
            result = (a & b ^ c) | d;
            Assert.Equal("a & b ^ c | d", result.ToString());
            result = (a ^ b & c) | d;
            Assert.Equal("a ^ b & c | d", result.ToString());

            result = a & (b | c ^ d);
            Assert.Equal("a & (b | c ^ d)", result.ToString());
            result = a | (b & c ^ d);
            Assert.Equal("a | b & c ^ d", result.ToString());
            result = a | (b ^ c & d);
            Assert.Equal("a | b ^ c & d", result.ToString());
            result = a ^ (b | c & d);
            Assert.Equal("a ^ (b | c & d)", result.ToString());
            result = a & (b ^ c | d);
            Assert.Equal("a & (b ^ c | d)", result.ToString());
            result = a ^ (b & c | d);
            Assert.Equal("a ^ (b & c | d)", result.ToString());
        }



        [Fact]
        public void TestKeepExistingParenthesis()
        {
            var a = new Formula<string>(new Expression<string>("a"));
            var b = new Formula<string>(new Expression<string>("b"));
            var c = new Formula<string>(new Expression<string>("c"));
            var d = new Formula<string>(new Expression<string>("d"));

            var result = (a & b) | c ^ d;
            Assert.Equal("(a) & (b) | (c) ^ (d)", result.ToString());
        }

        [Fact]
        public void TestWherePredicate()
        {
            var a = new Expression<object>("a") & new Expression<object>(1);

            var result = a.Where(value => value is string s && s == "a");
            Assert.Equal(new Formula<object>(new Expression<object>("a")), result);

            result = a.Where(value => value is string s && s == "b");
            Assert.Equal(new Formula<object>(), result);
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
