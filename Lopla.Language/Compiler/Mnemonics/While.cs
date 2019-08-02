using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class While : Mnemonic
    {
        public While(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            var logicalTest = node.GetChildAndAddError(1, "expression", runtime, false);
            LogicalExpression = runtime.Get(logicalTest);

            var blockOfLopla = node.GetChildAndAddError(2, "block_of_lopla", runtime, false);
            CodeBlock = runtime.Get(blockOfLopla);
        }

        public Mnemonic CodeBlock { get; set; }

        public Mnemonic LogicalExpression { get; set; }

        public override Result Execute(IRuntime runtime)
        {
            Result result = null;

            while (WhileCondition(runtime.EvaluateCodeBlock(LogicalExpression), runtime))
            {
                result = runtime.EvaluateCodeBlock(CodeBlock);
                if (runtime.ProcessingStopped())
                {
                    break;
                }
            }

            return result;
        }

        private bool WhileCondition(Result result, IRuntime runtime)
        {
            if (result.HasResult() &&
                result.Get(runtime) is Number resultInt)
            {
                return resultInt.Value == 1;
            }
            runtime.AddError(
                new RuntimeError($"Invalid data after evaulation of expression to use as a while condition.", this));
            return false;
        }
    }
}