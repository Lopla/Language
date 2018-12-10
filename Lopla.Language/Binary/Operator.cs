namespace Lopla.Language.Binary
{
    public enum OperatorType
    {
        Equals,
        NotEquals,
        Add,
        Substract,
        Divide,
        LessThen,
        GreaterThen,
        LessThenOrEqual,
        GreaterThenOrEqual,
        Multiply,
        Or,
        And
    }

    public class Operator : IArgument
    {
        public OperatorType Kind { get; set; }
    }
}