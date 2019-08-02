using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Hime.Redist;

    public class ExpressionBracket : Mnemonic
    {
        public ExpressionBracket(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            if (node?.Children.Count == 1)
            {
                var expresionToken = node?.Children[0];

                SubExpresion = runtime.Get(expresionToken);
            }
        }

        public Mnemonic SubExpresion { get; set; }

        public override Result Execute(IRuntime runtime)
        {
            var left = runtime.EvaluateCodeBlock(SubExpresion);

            return left;
        }
    }
}