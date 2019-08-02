namespace Lopla.Language.Environment
{
    public class Scope
    {
        public readonly Memory Mem = new Memory();
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name} mem - {Mem}";
        }
    }
}