namespace Lopla.Language.Binary
{
    public class VariablePointer : IArgument
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}