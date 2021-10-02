namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.Json;

    using PropositionalCalculus.BinaryOperators;
    using PropositionalCalculus.UnaryOperators;

    public class Parser
    {
        private IDictionary<string, UnaryOperator> _unaryOperators;

        [DisallowNull]
        public IDictionary<string, UnaryOperator> UnaryOperators
        {
            get
            {
                this._unaryOperators ??= new Dictionary<string, UnaryOperator>
                {
                    {UnaryOperator.NOT.ToString(), UnaryOperator.NOT},
                };
                return this._unaryOperators;
            }
            set => this._unaryOperators = value ?? throw new ArgumentNullException(nameof(this.UnaryOperators));
        }

        private IDictionary<string, BinaryOperators.BinaryOperator> _binaryOperators;

        [DisallowNull]
        public IDictionary<string, BinaryOperators.BinaryOperator> BinaryOperators
        {
            get
            {
                this._binaryOperators ??= new Dictionary<string, BinaryOperators.BinaryOperator>
                {
                    { BinaryOperator.AND.ToString(), BinaryOperator.AND },
                    { BinaryOperator.OR.ToString(), BinaryOperator.OR },
                    { BinaryOperator.XOR.ToString(), BinaryOperator.XOR },
                    { BinaryOperator.NAND.ToString(), BinaryOperator.NAND },
                    { BinaryOperator.NOR.ToString(), BinaryOperator.NOR },
                    { BinaryOperator.NXOR.ToString(), BinaryOperator.NXOR },
                };
                return this._binaryOperators;
            }
            set => this._binaryOperators = value ?? throw new ArgumentNullException(nameof(this.BinaryOperators));
        }

        public char EscapeCharacter { get; set; } = '\\';
        public (char, char) Brackets { get; set; } = ('(', ')');
        public bool Trim { get; set; } = true;

        public string BinaryToUnarySeparator = " ";
        public string UnaryToUnarySeparator = string.Empty;
        public string BinaryToExpressionSeparator = " ";
        public string UnaryToExpressionSeparator = string.Empty;
        public string BinaryToBracketSeparator = " ";
        public string UnaryToBracketSeparator = string.Empty;
        public string BracketToOtherBracketSeparator = " ";
        public string BracketToSameBracketSeparator = string.Empty;
        public string BracketToUnarySeparator = string.Empty;
        public string BracketToExpressionSeparator = string.Empty;

        public ExpressionOrFormula<T> Parse<T>(string input, Func<string, T> parseCallback)
        {
            throw new NotImplementedException();
        }

        private string ConvertToString<T>(
            ExpressionOrFormula<T> expressionOrFormula,
            Func<T, string> customToString,
            IDictionary<BinaryOperators.BinaryOperator, string> binaryOperators,
            IDictionary<UnaryOperators.UnaryOperator, string> unaryOperators)
        {
            if (expressionOrFormula == null)
            {
                return string.Empty;
            }

            customToString ??= (T obj) => obj.ToString();

            var str = expressionOrFormula.BinaryOperator == null ? string.Empty : binaryOperators[expressionOrFormula.BinaryOperator ?? BinaryOperator.INVALID_OPERATOR];
            if (expressionOrFormula.BinaryOperator != null && expressionOrFormula.UnaryOperators.Any())
            {
                str += this.BinaryToUnarySeparator;
            }

            str += string.Join(this.UnaryToUnarySeparator, expressionOrFormula.UnaryOperators.Select(uo => unaryOperators[uo]));

            if (expressionOrFormula is Expression<T> expression)
            {
                if (expression.UnaryOperators.Any())
                {
                    str += this.UnaryToExpressionSeparator;
                }
                else if (expression.BinaryOperator != null)
                {
                    str += this.BinaryToExpressionSeparator;
                }

                str += customToString(expression.Value);
            }
            else if (expressionOrFormula is Formula<T> formula)
            {
                if (formula.UnaryOperators.Any())
                {
                    str += this.UnaryToBracketSeparator;
                }
                else if (formula.BinaryOperator != null)
                {
                    str += this.BinaryToBracketSeparator;
                }

                str += this.Brackets.Item1;
                if (formula.ExpressionOrFormulas.Any())
                {
                    var first = formula.ExpressionOrFormulas.First();
                    str += first.UnaryOperators.Any() ? this.BracketToUnarySeparator : this.BracketToExpressionSeparator;

                    str += this.ConvertToString(first, customToString, binaryOperators, unaryOperators);
                    var lastWasBracket = first is Formula<T>;

                    foreach (var nxt in formula.ExpressionOrFormulas.Skip(1))
                    {
                        str += lastWasBracket ? this.BinaryToBracketSeparator : this.BinaryToExpressionSeparator;
                        str += this.ConvertToString(nxt, customToString, binaryOperators, unaryOperators);
                        lastWasBracket = nxt is Formula<T>;
                    }

                    str += lastWasBracket ? this.BracketToSameBracketSeparator : this.BracketToExpressionSeparator;
                }
                else
                {
                    str += this.BracketToOtherBracketSeparator;
                }
                str += this.Brackets.Item2;
            }

            return str;
        }

        public string ConvertToString<T>(ExpressionOrFormula<T> expressionOrFormula, Func<T, string> customToString)
        {
            var binaryOperators = this.BinaryOperators.ToDictionary(kv => kv.Value, kv => kv.Key);
            var unaryOperators = this.UnaryOperators.ToDictionary(kv => kv.Value, kv => kv.Key);

            return this.ConvertToString(expressionOrFormula, customToString, binaryOperators, unaryOperators);
        }

        public string ConvertToString<T>(ExpressionOrFormula<T> expressionOrFormula)
        {
            return this.ConvertToString(expressionOrFormula, (T obj) => obj.ToString());
        }

        public ExpressionOrFormula<T> ParseJson<T>(string input)
        {
            return this.Parse(input, arg => JsonSerializer.Deserialize<T>(arg));
        }

        public ExpressionOrFormula<string> ParseString(string input)
        {
            return this.Parse(input, arg => arg);
        }

        public ExpressionOrFormula<bool> ParseBool(string input)
        {
            return this.Parse(input, bool.Parse);
        }

        public ExpressionOrFormula<byte> ParseByte(string input)
        {
            return this.Parse(input, byte.Parse);
        }

        public ExpressionOrFormula<sbyte> ParseSbyte(string input)
        {
            return this.Parse(input, sbyte.Parse);
        }

        public ExpressionOrFormula<char> ParseChar(string input)
        {
            return this.Parse(input, char.Parse);
        }

        public ExpressionOrFormula<decimal> ParseDecimal(string input)
        {
            return this.Parse(input, decimal.Parse);
        }

        public ExpressionOrFormula<double> ParseDouble(string input)
        {
            return this.Parse(input, double.Parse);
        }

        public ExpressionOrFormula<float> ParseFloat(string input)
        {
            return this.Parse(input, float.Parse);
        }

        public ExpressionOrFormula<int> ParseInt(string input)
        {
            return this.Parse(input, int.Parse);
        }

        public ExpressionOrFormula<uint> ParseUint(string input)
        {
            return this.Parse(input, uint.Parse);
        }

        public ExpressionOrFormula<nint> ParseNint(string input)
        {
            return this.Parse(input, nint.Parse);
        }

        public ExpressionOrFormula<nuint> ParseNuint(string input)
        {
            return this.Parse(input, nuint.Parse);
        }

        public ExpressionOrFormula<long> ParseLong(string input)
        {
            return this.Parse(input, long.Parse);
        }

        public ExpressionOrFormula<ulong> ParseUlong(string input)
        {
            return this.Parse(input, ulong.Parse);
        }

        public ExpressionOrFormula<short> ParseShort(string input)
        {
            return this.Parse(input, short.Parse);
        }

        public ExpressionOrFormula<ushort> ParseUshort(string input)
        {
            return this.Parse(input, ushort.Parse);
        }
    }
}
