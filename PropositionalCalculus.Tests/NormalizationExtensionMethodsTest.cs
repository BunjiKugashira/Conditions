namespace PropositionalCalculus.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using PropositionalCalculus.BinaryOperators;

    using Xunit;

    public class NormalizationExtensionMethodsTest
    {
        [Fact]
        public void TestToConjunctiveNormalForm()
        {

        }

        [Fact]
        public void TestIsConjunctiveNormalForm()
        {

        }

        [Fact]
        public void TestToDisjunctiveNormalForm()
        {

        }

        [Fact]
        public void TestIsDisjunctiveNormalForm()
        {

        }

        [Fact]
        public void TestToNegationNormalForm()
        {

        }

        [Fact]
        public void TestIsNegationNormalForm()
        {

        }

        [Fact]
        public void TestToCanonicalNormalForm()
        {

        }

        [Fact]
        public void TestIsCanonicalNormalForm()
        {

        }

        [Fact]
        public void TestIsRedundant()
        {
            var a = new Formula<string>();
            Assert.True(a.IsRedundant(null));
            Assert.True(a.IsRedundant(And.Instance));

            a = new Formula<string>(And.Instance);
            Assert.True(a.IsRedundant(null));
            Assert.True(a.IsRedundant(And.Instance));

            a = new Formula<string>(new Expression<string>("a"));
            Assert.True(a.IsRedundant(null));
            Assert.True(a.IsRedundant(And.Instance));

            a = new Formula<string>(And.Instance, new Expression<string>("a"));
            Assert.True(a.IsRedundant(null));
            Assert.True(a.IsRedundant(And.Instance));

            a = new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b"));
            Assert.True(a.IsRedundant(null));
            Assert.True(a.IsRedundant(And.Instance));

            a = new Formula<string>(And.Instance, new Expression<string>("a"), new Expression<string>(And.Instance, "b"));
            Assert.True(a.IsRedundant(null));
            Assert.True(a.IsRedundant(And.Instance));

            a = new Formula<string>(new Expression<string>("a"), new Expression<string>(Or.Instance, "b"));
            Assert.True(a.IsRedundant(null));
            Assert.False(a.IsRedundant(And.Instance));

            a = new Formula<string>(And.Instance, new Expression<string>("a"), new Expression<string>(Or.Instance, "b"));
            Assert.False(a.IsRedundant(null));
            Assert.False(a.IsRedundant(And.Instance));

            a = new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b"));
            Assert.True(a.IsRedundant(null));
            Assert.True(a.IsRedundant(Or.Instance));

            a = new Formula<string>(Or.Instance, new Expression<string>("a"), new Expression<string>(And.Instance, "b"));
            Assert.True(a.IsRedundant(null));
            Assert.True(a.IsRedundant(Or.Instance));
        }

        [Fact]
        public void TestRemoveRedundantParenthesis()
        {
            var a = new Formula<string>(new Expression<string>("a"));
            Assert.Equal(new Expression<string>("a"), a.RemoveRedundantParenthesis());

            a = new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b"));
            Assert.Equal(a, a.RemoveRedundantParenthesis());

            a = new Formula<string>(new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b")), new Expression<string>(And.Instance, "c"));
            Assert.Equal(new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b"), new Expression<string>(And.Instance, "c")), a.RemoveRedundantParenthesis());

            a = new Formula<string>(new Formula<string>(new Expression<string>("a"), new Expression<string>(Or.Instance, "b")), new Expression<string>(And.Instance, "c"));
            Assert.Equal(a, a.RemoveRedundantParenthesis());

            a = new Formula<string>(new Expression<string>("a"), new Formula<string>(And.Instance, new Expression<string>("b"), new Expression<string>(And.Instance, "c")));
            Assert.Equal(new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b"), new Expression<string>(And.Instance, "c")), a.RemoveRedundantParenthesis());

            a = new Formula<string>(new Expression<string>("a"), new Formula<string>(And.Instance, new Expression<string>("b"), new Expression<string>(Or.Instance, "c")));
            Assert.Equal(a, a.RemoveRedundantParenthesis());
        }

        [Fact]
        public void TestHasRedundantParenthesis()
        {
            var a = new Formula<string>(new Expression<string>("a"));
            Assert.True(a.HasRedundantParenthesis());

            a = new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b"));
            Assert.False(a.HasRedundantParenthesis());

            a = new Formula<string>(new Formula<string>(new Expression<string>("a"), new Expression<string>(And.Instance, "b")), new Expression<string>(And.Instance, "c"));
            Assert.True(a.HasRedundantParenthesis());

            a = new Formula<string>(new Formula<string>(new Expression<string>("a"), new Expression<string>(Or.Instance, "b")), new Expression<string>(And.Instance, "c"));
            Assert.False(a.HasRedundantParenthesis());

            a = new Formula<string>(new Expression<string>("a"), new Formula<string>(And.Instance, new Expression<string>("b"), new Expression<string>(And.Instance, "c")));
            Assert.True(a.HasRedundantParenthesis());

            a = new Formula<string>(new Expression<string>("a"), new Formula<string>(And.Instance, new Expression<string>("b"), new Expression<string>(Or.Instance, "c")));
            Assert.False(a.HasRedundantParenthesis());

            a = new Formula<string>(new Expression<string>("a"), new Formula<string>(And.Instance, new Expression<string>("b"), new Expression<string>(Or.Instance, "c"), new Expression<string>(And.Instance, "d"), new Expression<string>(Or.Instance, "e")));
            Assert.False(a.HasRedundantParenthesis());

            a = new Formula<string>(new Expression<string>("a"), new Formula<string>(Xor.Instance, new Expression<string>("b"), new Expression<string>(Or.Instance, "c"), new Expression<string>(And.Instance, "d"), new Expression<string>(Or.Instance, "e")));
            Assert.False(a.HasRedundantParenthesis());
        }
    }
}
