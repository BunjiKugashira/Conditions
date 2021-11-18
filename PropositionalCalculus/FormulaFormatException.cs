namespace PropositionalCalculus
{
    using System;

    public class FormulaFormatException : FormatException
    {
        public int Position { get; }

        public FormulaFormatException(string message, int position)
        : base(message)
        {
            this.Position = position;
        }
    }
}
