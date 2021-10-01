namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;

    public class Parser
    {
        private IDictionary<string, UnaryOperatorEnum> _unaryOperators;

        [DisallowNull]
        public IDictionary<string, UnaryOperatorEnum> UnaryOperators
        {
            get
            {
                this._unaryOperators ??= new Dictionary<string, UnaryOperatorEnum>
                {
                    {"!", UnaryOperatorEnum.NOT},
                };
                return this._unaryOperators;
            }
            set => this._unaryOperators = value ?? throw new ArgumentNullException(nameof(UnaryOperators));
        }

        private IDictionary<string, BinaryOperatorEnum> _binaryOperators;

        [DisallowNull]
        public IDictionary<string, BinaryOperatorEnum> BinaryOperators
        {
            get
            {
                this._binaryOperators ??= new Dictionary<string, BinaryOperatorEnum>
                {
                    {"&&", BinaryOperatorEnum.AND},
                    {"||", BinaryOperatorEnum.OR},
                    {"^", BinaryOperatorEnum.XOR},
                };
                return this._binaryOperators;
            }
            set => this._binaryOperators = value ?? throw new ArgumentNullException(nameof(BinaryOperators));
        }

        public char EscapeCharacter { get; set; } = '\\';
        public (char, char) Brackets { get; set; } = ('(', ')');
        public bool Trim { get; set; } = true;

        public ExpressionOrFormula<T> Parse<T>(string input, Func<string, T> parseCallback)
        {
            throw new NotImplementedException();
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
