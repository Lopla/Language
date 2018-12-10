namespace Lopla.Language.Binary
{
    using Hime.Redist;
    using Processing;

    public abstract class Mnemonic : IArgument
    {
        protected Mnemonic(ASTNode? node)
        {
            if (node.HasValue)
            {
                Symbol = node?.Symbol.Name;

                var n = node;
                do
                {
                    Line = n?.Position.Line;
                    Column = n?.Position.Column;

                    n = n.Value.Children.Count > 0 ? (ASTNode?) n.Value.Children[0] : null;
                } while (Line == 0 && n != null);
            }
        }

        public string Symbol { get; }
        public int? Line { get; }
        public int? Column { get; }
        public abstract Result Execute(Runtime runtime);

        public override string ToString()
        {
            return $"{Line}\t{Column}\t{Symbol}";
        }
    }
}