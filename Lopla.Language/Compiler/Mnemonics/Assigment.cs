namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Errors;
    using Hime.Redist;
    using Interfaces;
    using Processing;

    public class Assigment : Mnemonic
    {
        public Assigment(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            var variableNameNode = node.GetChildAndAddError(0, "variable_name", runtime, true);
            var varValueTable = node.GetChildAndAddError(0, "var_value_table", runtime, true);

            if (variableNameNode.HasValue)
                LeftSide = new VariableName(variableNameNode, runtime);
            else if (varValueTable.HasValue)
                LeftSide = new ValueTable(varValueTable, runtime);
            else
                runtime.AddError(new CompilationError(
                    $"Failed to find variable name in left side of assigment."));

            var expression =
                runtime.Get(node.Value.Children[2]);

            RightSide =
                expression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="leftSide">can be one of <see cref="VariableName"/> or <see cref="ValueTable"/></param>
        /// <param name="rightSide">usually some <see cref="Mnemonic"/></param>
        public Assigment(ASTNode? node, IArgument leftSide, Mnemonic rightSide) : base(node)
        {
            LeftSide = leftSide;
            RightSide = rightSide;
        }

        public IArgument LeftSide { get; set; }

        public Mnemonic RightSide { get; set; }

        public override Result Execute(IRuntime runtime)
        {
            var rightResult = runtime.EvaluateCodeBlock(RightSide);
            if (LeftSide is VariableName vn)
            {
                runtime.SetVariable(vn.Pointer.Name, rightResult);
            }
            else if (LeftSide is ValueTable vt)
            {
                var leftSideIndexedType = runtime.GetVariable(vt.TablePointer.Name);
                if (leftSideIndexedType != null)
                {
                    var value = leftSideIndexedType.Get(runtime);
                    if (value is ILoplaIndexedValue)
                    {
                        var loplaList = value as ILoplaIndexedValue;
                        var idx = runtime.EvaluateCodeBlock(vt.ElementPositionInTable).Get(runtime) as Number;
                        loplaList.Set(idx.ValueAsInt, rightResult);
                        runtime.SetVariable(vt.TablePointer.Name, new Result(loplaList));
                    }
                    else
                    {
                        runtime.AddError(new RuntimeError($"Cannot handle {vt.TablePointer.Name} like an array.",
                            this));
                    }
                }
                else
                {
                    runtime.AddError(new RuntimeError($"Table {vt.TablePointer.Name} must be initialized before use.",
                        this));
                }
            }

            return new Result();
        }
    }
}