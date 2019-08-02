using Lopla.Language.Interfaces;

namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Hime.Redist;
    using Processing;

    public class ValueString : Mnemonic
    {
        public String Value;

        public ValueString(ASTNode? node) : base(node)
        {
            Value = new String
            {
                Value = node.Value.Value?.Trim('\"')
            };
        }

        public ValueString(ASTNode? node, String value) : base(node)
        {
            Value = value;
        }

        public override Result Execute(IRuntime runtime)
        {
            return new Result(Value);
        }
    }
}