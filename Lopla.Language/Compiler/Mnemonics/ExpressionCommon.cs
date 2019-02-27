using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Errors;
    using Hime.Redist;

    public class ExpressionCommon : Mnemonic
    {

        public Arguments Arguments { get; }

        public ExpressionCommon(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            Arguments=new Arguments();

            if (node?.Children.Count == 3)
            {
                var left = node?.Children[0];
                var middle = node?.Children[1];
                var right = node?.Children[2];

                var leftExpression = runtime.Get(left);
                var operatorKind = middle.Value.Value;
                var rightExpression = runtime.Get(right);

                var o = new Operator();

                if (Expression.Operators.ContainsKey(operatorKind))
                    o.Kind = Expression.Operators[operatorKind];
                else
                    runtime.AddError(
                        new CompilationError($"Operator {operatorKind} is not discovered by compiler."));
                Arguments.Args.Add(leftExpression);
                Arguments.Args.Add(o);
                Arguments.Args.Add(rightExpression);
            }
            else if(node?.Children.Count == 1)
            {
                this.Arguments.Args.Add(runtime.Get(node.Value.Children[0]));
            }
        }

        public override Result Execute(Runtime runtime)
        {
            var left = runtime.EvaluateCodeBlock(Arguments.Args[0]);
            if (Arguments.Args.Count > 1)
                if (Arguments.Args[1] is Operator op)
                {
                    var right = runtime.EvaluateCodeBlock(Arguments.Args[2]);
                    return  Expression.ArgumentCalcualte(this, left, right, op.Kind, runtime);
                }
                else
                {
                    runtime.AddError(new RuntimeError("Operator not provided.", this));
                }

            return left;
        }
    }
}