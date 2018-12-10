namespace Lopla.Language.Binary
{
    public interface IValue
    {
        IValue Clone();
    }

    public interface IValue<T> : IValue
    {
        T Value { get; set; }
    }
}