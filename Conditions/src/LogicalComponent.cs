namespace Conditions
{
    public abstract class LogicalComponent
    {
        public LogicalComponent PreviousComponent { get; set; }
        public LogicalComponent NextComponent { get; set; }
    }
}