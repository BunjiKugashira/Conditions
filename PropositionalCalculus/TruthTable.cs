namespace PropositionalCalculus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using PropositionalCalculus.BinaryOperators;
    using PropositionalCalculus.UnaryOperators;

    public class TruthTable<T>
    {
        public class Row
        {
            public IReadOnlyDictionary<T, bool> InputValues { get; }

            public bool OutputValue { get; }

            public TruthTable<T> Table { get; }

            internal Row(TruthTable<T> table, IEnumerable<KeyValuePair<T, bool>> inputValues, bool outputValue)
            {
                if (table.ValidateInputs)
                {
                    if (inputValues.Count() != table.NumberOfDistinctExpressions)
                        throw new ArgumentException($@"Expected {table.NumberOfDistinctExpressions} distinct expressions, but found only {inputValues.Count()} expressions.");
                    foreach (var problem in inputValues.GroupBy(kv => kv.Key).Where(kv => kv.Count() > 1))
                    {
                        throw new ArgumentException($@"Expression {problem.Key} is not distinct. It is included {problem.Count()} times. Allowed is exactly once.");
                    }
                    foreach (var problem in inputValues.Where(kv => kv.Key == null))
                    {
                        throw new ArgumentNullException($@"Parameter {nameof(inputValues)} must not include null as a key.");
                    }
                }

                this.Table = table;
                this.InputValues = inputValues.ToDictionary(kv => kv.Key, kv => kv.Value);
                this.OutputValue = outputValue;
            }
        }

        public bool ValidateInputs { get; set; } = true;

        private IList<Row> _rows = new List<Row>();
        public IEnumerable<Row> Rows { get => this._rows; }

        public int NumberOfDistinctExpressions { get; }

        public int NumberOfRows { get => (int)Math.Pow(2, this.NumberOfDistinctExpressions); }

        public bool IsComplete { get => this.Rows.Count() == this.NumberOfRows; }

        public TruthTable(int size)
        {
            this.NumberOfDistinctExpressions = size;
        }

        public TruthTable(ExpressionOrFormula<T> expressionOrFormula)
        {
            var elements = expressionOrFormula.GetAllElements().ToArray();
            this.NumberOfDistinctExpressions = elements.Length;

            for(var i = 0; i < this.NumberOfRows; i++)
            {
                var inputValues = elements.Select((element, elementIndex) => new KeyValuePair<T, bool>(element, i / (int)Math.Pow(2, elementIndex) % 2 == 0));
                // TODO Get Result for Input
                var outputValue = false;
                AddRow(inputValues, outputValue);
            }
        }

        public void AddRow(IEnumerable<KeyValuePair<T, bool>> inputValues, bool outputValue)
        {
            var row = new Row(this, inputValues, outputValue);

            if (this.ValidateInputs)
            {
                var problems = this.Rows.Where(row => row.InputValues.All(iv => iv.Value == inputValues.First(x => x.Key.Equals(iv.Key)).Value));
                foreach(var problem in problems)
                {
                    throw new ArgumentException("The given input combination is already registered in the table.");
                }
            }

            this._rows.Add(row);
        }

        public ExpressionOrFormula<T> ToExpressionOrFormula()
        {
            if (this.ValidateInputs)
            {
                if (!this.IsComplete)
                {
                    throw new InvalidOperationException($@"Truth table must be complete before converting to ExpressionOrFormula. So far {this.Rows.Count()} rows out of {this.NumberOfRows} required rows have been added.");
                }
            }

            var formulas = this.Rows
                .Select((row, rowIndex) =>
                {
                    var expressions = row.InputValues
                    .Select((iv, ivIndex) => new Expression<T>(
                        ivIndex == 0 ? null : And.Instance, 
                        iv.Value ? Enumerable.Empty<UnaryOperator>() : new UnaryOperator[] { Not.Instance }, 
                        iv.Key))
                    .ToArray();
                    return new Formula<T>(
                        rowIndex == 0 ? null : Or.Instance,
                        row.OutputValue ? Enumerable.Empty<UnaryOperator>() : new UnaryOperator[] { Not.Instance },
                        expressions);
                })
                .ToArray();

            return new Formula<T>(formulas);
        }
    }
}
