namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Errors;
    using Hime.Redist;
    using Interfaces;
    using Processing;

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

        public ValueTable(ASTNode? node, VariablePointer tablePointer, Mnemonic positionInTable) : base(node)
        {
            this.TablePointer = tablePointer;
            this.ElementPositionInTable = positionInTable;
        }

        public VariablePointer TablePointer { get; set; }

        public Mnemonic ElementPositionInTable { get; set; }

        public override Result Execute(IRuntime runtime)
        {
            var value = runtime.GetVariable(TablePointer.Name);
            if (value != null && value.HasResult())
            {
                var variable = value.Get(runtime);
                var idx = runtime.EvaluateCodeBlock(ElementPositionInTable).Get(runtime) as Number;

                if (variable is LoplaList tableInstance) return tableInstance.Get(idx);

                if (variable is String variableString)
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