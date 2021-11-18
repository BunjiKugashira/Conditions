namespace PropositionalCalculus.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using PropositionalCalculus.BinaryOperators;
    using PropositionalCalculus.UnaryOperators;

    using Xunit;

    public class NormalizationExtensionMethodsTest
    {
        [Fact]
        public void TestToConjunctiveNormalForm()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIsConjunctiveNormalForm()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestToDisjunctiveNormalForm()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIsDisjunctiveNormalForm()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestToNegationNormalForm()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIsNegationNormalForm()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestToCanonicalNormalForm()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIsCanonicalNormalForm()
        {
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("a & b", "a & b")]
        [InlineData("(a)", "a")]
        [InlineData("(a & b)", "a & b")]
        [InlineData("(a) & b", "a & b")]
        [InlineData("a & (b)", "a & b")]
        [InlineData("a & (b | c)", "a & b | a & c")]
        [InlineData("a | (b & c)", "a | b & c")]
        [InlineData("(a & b) | c", "a & b | c")]
        [InlineData("(a | b) & c", "a & c | b & c")]
        [InlineData("(a | b) & (c | d)", "a & b | a & d | b & c | b & d")]
        [InlineData("(a & b | c) ^ d", "a & b ^ d | c ^ d")]
        public void TestFlatten_ExpectGivenOutput(string input, string expectedOutput)
        {
            // Arrange
            var parser = new Parser();
            var inputExpressionOrFormula = parser.ParseString(input);

            // Act
            var outputExpressionOrFormula = inputExpressionOrFormula.Flatten();
            var result = parser.ConvertToString(outputExpressionOrFormula);

            // Assert
            Assert.Equal(expectedOutput, result);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("a & b")]
        [InlineData("a | b")]
        [InlineData("a & b | c")]
        public void TestIsFlat_ExpectTrue(string input)
        {
            // Arrange
            var parser = new Parser();
            var expressionOrFormula = parser.ParseString(input);

            // Act
            var result = expressionOrFormula.IsFlat();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("( )")]
        [InlineData("(a)")]
        [InlineData("(a & b)")]
        [InlineData("(a & b) & c")]
        [InlineData("a & (b & c)")]
        public void TestIsFlat_ExpectFalse(string input)
        {
            // Arrange
            var parser = new Parser();
            var expressionOrFormula = parser.ParseString(input);

            // Act
            var result = expressionOrFormula.IsFlat();

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("()")]
        [InlineData("(a)")]
        [InlineData("(a & b)")]
        [InlineData("(a | b)")]
        [InlineData("a & b")]
        [InlineData("a | b")]
        public void TestIsRedundant_WithoutOperator_ExpectTrue(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input);

            // Act
            var result = formula.IsRedundant(null);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("()")]
        [InlineData("(a & b)")]
        [InlineData("(a | b)")]
        [InlineData("(a ^ b)")]
        [InlineData("a & b")]
        public void TestIsRedundant_WithAnd_ExpectTrue(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input);

            // Act
            var result = formula.IsRedundant(And.Instance);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("a | b")]
        [InlineData("a ^ b")]
        public void TestIsRedundant_WithAnd_ExpectFalse(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input);

            // Act
            var result = formula.IsRedundant(And.Instance);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("()")]
        [InlineData("(a & b)")]
        [InlineData("(a | b)")]
        [InlineData("(a ^ b)")]
        [InlineData("a & b")]
        public void TestIsRedundant_WithLeadingAnd_ExpectTrue(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input).WithOperators(And.Instance, Enumerable.Empty<UnaryOperator>());

            // Act
            var result = formula.IsRedundant(null);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("a | b")]
        [InlineData("a ^ b")]
        public void TestIsRedundant_WithLeadingAnd_ExpectFalse(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input).WithOperators(And.Instance, Enumerable.Empty<UnaryOperator>());

            // Act
            var result = formula.IsRedundant(null);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("()")]
        [InlineData("(a & b)")]
        [InlineData("(a | b)")]
        [InlineData("(a ^ b)")]
        [InlineData("a & b")]
        [InlineData("a | b")]
        [InlineData("a ^ b")]
        public void TestIsRedundant_WithOr_ExpectTrue(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input);

            // Act
            var result = formula.IsRedundant(Or.Instance);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("()")]
        [InlineData("(a & b)")]
        [InlineData("(a | b)")]
        [InlineData("(a ^ b)")]
        [InlineData("a & b")]
        [InlineData("a | b")]
        [InlineData("a ^ b")]
        public void TestIsRedundant_WithLeadingOr_ExpectTrue(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input).WithOperators(Or.Instance, Enumerable.Empty<UnaryOperator>());

            // Act
            var result = formula.IsRedundant(null);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("()")]
        [InlineData("(a & b)")]
        [InlineData("(a | b)")]
        [InlineData("(a ^ b)")]
        [InlineData("(a & b | c)")]
        [InlineData("a & b")]
        [InlineData("a ^ b")]
        public void TestIsRedundant_WithXor_ExpectTrue(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input);

            // Act
            var result = formula.IsRedundant(Xor.Instance);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("a | b")]
        [InlineData("a & b | c")]
        public void TestIsRedundant_WithXor_ExpectFalse(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input);

            // Act
            var result = formula.IsRedundant(Xor.Instance);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("()")]
        [InlineData("(a & b)")]
        [InlineData("(a | b)")]
        [InlineData("(a ^ b)")]
        [InlineData("(a & b | c)")]
        [InlineData("a & b")]
        [InlineData("a ^ b")]
        public void TestIsRedundant_WithLeadingXor_ExpectTrue(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input).WithOperators(Xor.Instance, Enumerable.Empty<UnaryOperator>());

            // Act
            var result = formula.IsRedundant(null);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("a | b")]
        [InlineData("a & b | c")]
        public void TestIsRedundant_WithLeadingXor_ExpectFalse(string input)
        {
            // Arrange
            var parser = new Parser();
            var formula = (Formula<string>)parser.ParseString(input).WithOperators(Xor.Instance, Enumerable.Empty<UnaryOperator>());

            // Act
            var result = formula.IsRedundant(null);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("(a)", "a")]
        [InlineData("(!a)", "!a")]
        [InlineData("(a & b)", "a & b")]
        [InlineData("(a & b) & c", "a & b & c")]
        [InlineData("a & (b & c)", "a & b & c")]
        [InlineData("a & ((b | c))", "a & (b | c)")]
        [InlineData("(a & b) ^ (c | d)", "a & b ^ (c | d)")]
        [InlineData("(a | b) ^ (c & d)", "(a | b) ^ c & d")]
        public void TestRemoveRedundantParenthesis_ExpectGivenOutput(string input, string expectedOutput)
        {
            // Arrange
            var parser = new Parser();
            var inputExpressionOrFormula = parser.ParseString(input);

            // Act
            var outputExpressionOrFormula = inputExpressionOrFormula.RemoveRedundantParenthesis();
            var output = parser.ConvertToString(outputExpressionOrFormula);

            // Assert
            Assert.Equal(expectedOutput, output);
        }

        [Theory]
        [InlineData("( )")]
        [InlineData("(a)")]
        [InlineData("(!a)")]
        [InlineData("(a & b)")]
        [InlineData("(a & b) & c")]
        [InlineData("a & (b & c)")]
        [InlineData("a & ((b | c))")]
        [InlineData("(a & b) ^ (c | d)")]
        [InlineData("(a | b) ^ (c & d)")]
        public void TestHasRedundantParenthesis_ExpectTrue(string input)
        {
            // Arrange
            var parser = new Parser();
            var expressionOrFormula = parser.ParseString(input);

            // Act
            var result = expressionOrFormula.HasRedundantParenthesis();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("a & b")]
        [InlineData("a & (b | c)")]
        [InlineData("(a | b) & c")]
        [InlineData("(a | b) & (c | d)")]
        [InlineData("a ^ (b & c | d)")]
        [InlineData("(a & b | c) ^ d")]
        public void TestHasRedundantParenthesis_ExpectFalse(string input)
        {
            // Arrange
            var parser = new Parser();
            var expressionOrFormula = parser.ParseString(input);

            // Act
            var result = expressionOrFormula.HasRedundantParenthesis();

            // Assert
            Assert.False(result);
        }
    }
}
