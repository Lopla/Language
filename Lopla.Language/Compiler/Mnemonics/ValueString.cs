using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Enviorment;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    internal class ValueString : Mnemonic
    {
        public String Value;

        public ValueString(ASTNode? node) : base(node)
        {
            Value = new String
            {
                Value = node.Value.Value?.Trim('\"')
            };
        }

        public override Result Execute(Runtime runtime)
        {
            return new Result(Value);
        }
    }
}