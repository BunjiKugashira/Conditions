namespace PropositionalCalculus.Tests
{
    using PropositionalCalculus;

    using Xunit;

    public class ParserTest
    {
        [Theory]
        [InlineData("( )")]
        [InlineData("(a)")]
        [InlineData("((a))")]
        [InlineData("a & b")]
        [InlineData("a | b")]
        [InlineData("a ^ b")]
        [InlineData("(a & b)")]
        [InlineData("(a | b)")]
        [InlineData("(a ^ b)")]
        [InlineData("a & (b & c)")]
        [InlineData("(a & b) & c")]
        [InlineData("(a & b) & (c & d)")]
        [InlineData("a & !(b & c)")]
        [InlineData("!(a & b) & c")]
        [InlineData("(!((!a | b) & (a | !b)) | (!c | !b)) & !c")]
        public void TestDefaultParse_ShouldReturnFormula(string input)
        {
            // Arrange
            var parser = new Parser();

            // Act
            var expressionOrFormula = parser.ParseString(input);
            var output = parser.ConvertToString(expressionOrFormula);

            // Assert
            Assert.IsType<Formula<string>>(expressionOrFormula);
            Assert.Equal(input, output);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("!a")]
        [InlineData("!!a")]
        [InlineData("long test string")]
        public void TestDefaultParse_ShouldReturnExpression(string input)
        {
            // Arrange
            var parser = new Parser();

            // Act
            var expressionOrFormula = parser.ParseString(input);
            var output = parser.ConvertToString(expressionOrFormula);

            // Assert
            Assert.IsType<Expression<string>>(expressionOrFormula);
            Assert.Equal(input, output);
        }

        [Theory]
        [InlineData("")]
        public void TestDefaultParse_ShouldThrow(string input)
        {
            // Arrange
            var parser = new Parser();

            // Assert
            Assert.Throws<FormulaFormatException>(
                // Act
                () => parser.ParseString(input)
                );
        }
    }
}
