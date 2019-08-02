namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Hime.Redist;
    using Interfaces;
    using Processing;

    public class VariableName : Mnemonic
    {
        public VariableName(ASTNode? node, string name) : base(node)
        {
            this.Pointer = new VariablePointer()
            {
                Name = name
            };
        }

        public VariableName(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            Pointer = new VariablePointer
            {
                Name = node.GetChildAndAddError(0, "LITERAL", runtime)?.Value
            };
        }

        public VariablePointer Pointer { get; set; }

        public override Result Execute(IRuntime runtime)
        {
            var value = runtime.GetVariable(Pointer.Name);
            return value;
        }

        public override string ToString()
        {
            return $"Variable {Pointer}";
        }
    }
}