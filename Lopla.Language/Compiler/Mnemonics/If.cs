using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class If : Mnemonic
    {
        public If(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            var logicalTest = node.GetChildAndAddError(1, "expression", runtime, false);
            Condition = runtime.Get(logicalTest);

            var blockOfLopla = node.GetChildAndAddError(2, "block_of_lopla", runtime, false);
            BlockOfCode = runtime.Get(blockOfLopla);
        }

        public Mnemonic Condition { get; set; }

        public Mnemonic BlockOfCode { get; set; }

        public override Result Execute(IRuntime runtime)
        {
            var ifcheck = runtime.EvaluateCodeBlock(Condition);

            Result result = null;

            if (ifcheck?.HasResult() == true &&
                ifcheck.Get(runtime) is Number resultInt)
            {
                if (resultInt.Value != 0)
                    result = runtime.EvaluateCodeBlock(BlockOfCode);
            }
            else
            {
                runtime.AddError(new RuntimeError(
                    $"Invalid data after evaulation of if check expression. Exptected {nameof(Number)}. "
                    , this));
            }

            return result;
        }

        public override string ToString()
        {
            return $"if ({Condition})";
        }
    }
}