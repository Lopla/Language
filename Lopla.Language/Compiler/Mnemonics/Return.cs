namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Hime.Redist;
    using Interfaces;
    using Processing;

    public class Return : Mnemonic
    {
        public Return(ASTNode? node, Mnemonic expression) : base(node)
        {
            ReturnExpression = expression;
        }

        public Return(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            var returnValue =
                node.GetChildAndAddError(1, "expression", runtime, true);
            if (returnValue == null)
                ReturnExpression = new Nop(node);
            else
                ReturnExpression = runtime.Get(returnValue);
        }

        public Mnemonic ReturnExpression { get; set; }

        public override Result Execute(Runtime runtime)
        {
            var result = runtime.EvaluateCodeBlock(ReturnExpression);

            runtime.FunctionReturn(result);
            return result;
        }

        public override string ToString()
        {
            return $"return {ReturnExpression}";
        }
    }
}