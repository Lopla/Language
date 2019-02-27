using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Hime.Redist;

    public class ExpressionPrefix : Mnemonic
    {
        public ExpressionPrefix(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            if (node?.Children.Count == 2)
            {
                var right = node?.Children[1];
                this.MultByMinus1= runtime.Get(right);
            }
            else
            {
                this.ExpressionToPrefix = runtime.Get(node.Value.Children[0]);
            }
        }

        public Mnemonic ExpressionToPrefix { get; set; }

        public Mnemonic MultByMinus1 { get; }

        public override Result Execute(Runtime runtime)
        {
            if (this.MultByMinus1 == null)
            {
                return runtime.EvaluateCodeBlock(this.ExpressionToPrefix);
            }
            else
            {
                var right = runtime.EvaluateCodeBlock(MultByMinus1);

                return Expression.ArgumentCalcualte(this, new Result(new Number(-1)), right, OperatorType.Multiply, runtime);
            }
        }

        public override string ToString()
        {
            return $"Sum";
        }
    }
}