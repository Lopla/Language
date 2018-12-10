namespace Lopla.Language.Binary
{
    public class MethodPointer : IArgument
    {
        public MethodPointer()
            : this(null, null)
        {
        }

        public MethodPointer(string name, string nameSpace)
        {
            Name = name;
            NameSpace = nameSpace;
        }

        public string Name { get; set; }
        public string NameSpace { get; set; }

        public override string ToString()
        {
            return $"{NameSpace}.{Name}";
        }
    }
}