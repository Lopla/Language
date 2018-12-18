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

        public override Result Execute(Runtime runtime)
        {
            var rightResult = runtime.EvaluateCodeBlock(RightSide);
            if (LeftSide is VariableName vn)
            {
                runtime.SetVariable(vn.Pointer.Name, rightResult);
            }
            else if (LeftSide is ValueTable vt)
            {
                var table = runtime.GetVariable(vt.TablePointer.Name);
                if (table != null)
                {
                    var r = table.Get(runtime) as LoplaList;
                    var idx = runtime.EvaluateCodeBlock(vt.ElementPositionInTable).Get(runtime) as Number;
                    r.Set(idx.ValueAsInt, rightResult);
                    runtime.SetVariable(vt.TablePointer.Name, new Result(r));
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