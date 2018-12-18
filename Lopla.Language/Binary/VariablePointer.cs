namespace Lopla.Language.Binary
{
    public class VariablePointer : IArgument
    {
        public VariablePointer()
        {

        }

        public VariablePointer(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}