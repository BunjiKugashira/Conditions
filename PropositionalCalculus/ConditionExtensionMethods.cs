namespace PropositionalCalculus
{
    public static class ConditionExtensionMethods
    {
        public static Condition First(this Condition condition)
        {
            while(condition.Previous != null)
            {
                condition = condition.Previous;
            }

            return condition;
        }

        public static Condition Last(this Condition condition)
        {
            while(condition.Next != null)
            {
                condition = condition.Next;
            }

            return condition;
        }

        public static Condition ToConjunctiveNormalForm(this Condition condition)
        {
            return condition;
        }

        public static Condition ToDisjunctiveNormalForm(this Condition condition)
        {
            return condition;
        }

        public static Condition ToNegationNormalForm(this Condition condition)
        {
            return condition;
        }

        public static Condition ToCanonicalNormalForm(this Condition condition)
        {
            return condition;
        }

        public static Condition RemoveDuplicateBrackets(this Condition condition)
        {
            return condition;
        }
    }
}
