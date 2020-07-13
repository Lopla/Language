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
            var value = runtime.GetReference(TablePointer.Name);

            if (value != null)
            {
                var idx = runtime.EvaluateCodeBlock(ElementPositionInTable).Get(runtime) as Number;
                
                if (value is ILoplaIndexedValue tableInstance) 
                    return tableInstance.Get(idx.ValueAsInt);
                else
                {
                    runtime.AddError(new RuntimeError($"Value {TablePointer.Name} is not an array", this));
                }
            }
            else
            {
                runtime.AddError(new RuntimeError($"Value not defined {TablePointer.Name}", this));
            }
            return new Result();
        }
    }
}