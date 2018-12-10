using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Enviorment;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class ValueTable : Mnemonic
    {
        public ValueTable(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            TablePointer = new VariablePointer
            {
                Name = node
                    .GetChildAndAddError(0, "variable_name", runtime)?
                    .GetChildAndAddError(0, "LITERAL", runtime)?.Value
            };
            ElementPositionInTable = runtime.Get(node.Value.Children[1]);
        }

        public VariablePointer TablePointer { get; set; }

        public Mnemonic ElementPositionInTable { get; set; }

        public override Result Execute(Runtime runtime)
        {
            var value = runtime.GetVariable(TablePointer.Name);
            if (value != null && value.HasResult())
            {
                var variable = value.Get(runtime);
                var idx = runtime.EvaluateCodeBlock(ElementPositionInTable).Get(runtime) as Number;

                if (variable is LoplaList tableInstance)
                {
                    return tableInstance.Get(idx);
                }
                else if (variable is String variableString)
                {
                    var c = (int) variableString.Value[idx.ValueAsInt];
                    return new Result(new Number(c));
                }
            }

            runtime.AddError(new RuntimeError($"Value not defined {TablePointer.Name}", this));
            return new Result();
        }
    }
}