using System;
using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    using System.Collections.Generic;
    using System.Linq;
    using Hime.Redist;
    using String = Binary.String;

    public class Expression : Mnemonic
    {
        public static Dictionary<string, OperatorType> Operators = new Dictionary<string, OperatorType>()
        {
            {"+", OperatorType.Add },
            {"-", OperatorType.Subtract },
            {"/", OperatorType.Divide },
            {"*", OperatorType.Multiply },

            {"==", OperatorType.Equals },
            {"!=", OperatorType.NotEquals },
            {"<", OperatorType.LessThen },
            {">", OperatorType.GreaterThen },
            {"<=", OperatorType.LessThenOrEqual },
            {">=", OperatorType.GreaterThenOrEqual },


            {"||", OperatorType.Or },
            {"&&", OperatorType.And },
        };

        public Expression(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            this.SubExpression = runtime.Get(node.Value.Children[0]);
        }

        public Mnemonic SubExpression { get; set; }

        public override Result Execute(Runtime runtime)
        {
            return runtime.EvaluateCodeBlock(this.SubExpression);
        }

        public static Result ArgumentCalcualte(Mnemonic ctx, Result left, Result right,
            OperatorType kind, Runtime runtime)
        {
            if (left.HasResult() && right.HasResult())
            {
                var leftArg = left.Get(runtime);
                var rightArg = right.Get(runtime);

                if (leftArg is String leftString && rightArg is String rightString)
                {
                    if (new[] { OperatorType.Equals, OperatorType.GreaterThen, OperatorType.GreaterThenOrEqual,
                        OperatorType.LessThen, OperatorType.LessThenOrEqual}.Contains(kind))
                    {
                        return ArgumentCalcualte<String, string>(leftString, rightString, kind);
                    }
                    else if (kind == OperatorType.Add)
                    {
                        return new Result(new String( Convert.ToString(leftString.Value) + Convert.ToString(rightString.Value)));
                    }else
                    {
                        runtime.AddError(new RuntimeError($"Operator not suppoerted {kind} on strings.", ctx));
                    }
                }
                else if (leftArg is Number leftInt && rightArg is Number rightInt)
                {
                    return ArgumentCalculate(leftInt, rightInt, kind, runtime, ctx);
                }
                else
                {
                    runtime.AddError(new RuntimeError($"Expected number or string on both sides of the {kind} operator.", ctx));
                }
            }
            else
            {
                runtime.AddError(new RuntimeError($"Too much or missing argument for {kind} operator.", ctx));
            }

            return new Result();
        }

        public static Result ArgumentCalcualte<T, TArg>(T leftInt, T rightInt,
            OperatorType kind)
            where T : IValue<TArg>
            where TArg : IEquatable<TArg>, IComparable<TArg>
        {
            var leftValue = leftInt.Value;
            var rightValue = rightInt.Value;

            if (kind == OperatorType.Equals)
            {
                var equal = leftValue.Equals(rightValue);
                return new Result(equal ? new Number(1) : new Number(0));
            }
            else if (kind == OperatorType.NotEquals)
            {
                var equal = ! leftValue.Equals(rightValue);
                return new Result(equal ? new Number(1) : new Number(0));
            }
            else if (kind == OperatorType.LessThen)
            {
                var compareResult = leftValue.CompareTo(rightValue);
                return new Result(compareResult < 0 ? new Number(1) : new Number(0));
            }
            else if (kind == OperatorType.GreaterThen)
            {
                var compareResult = leftValue.CompareTo(rightValue);
                return new Result(compareResult > 0 ? new Number(1) : new Number(0));
            }
            else if (kind == OperatorType.GreaterThenOrEqual)
            {
                var compareResult = leftValue.CompareTo(rightValue);
                return new Result(compareResult >= 0 ? new Number(1) : new Number(0));
            } if (kind == OperatorType.LessThenOrEqual)
            {
                var compareResult = leftValue.CompareTo(rightValue);
                return new Result(compareResult <= 0 ? new Number(1) : new Number(0));
            }

            return new Result();
        }

        public static Result ArgumentCalculate(Number leftInt, Number rightInt, OperatorType kind, Runtime runtime, Mnemonic ctx)
        {
            if (new[] { OperatorType.Equals, OperatorType.GreaterThen,
                            OperatorType.GreaterThenOrEqual,
                            OperatorType.LessThen, OperatorType.LessThenOrEqual,
                            OperatorType.NotEquals

                    }.Contains(kind))
            {
                return ArgumentCalcualte<Number, decimal>(leftInt, rightInt, kind);
            }
            else if (kind == OperatorType.Add)
            {
                return new Result(new Number(leftInt.Value + rightInt.Value));
            }
            else if (kind == OperatorType.Subtract)
            {
                return new Result(new Number(leftInt.Value - rightInt.Value));
            }
            else if (kind == OperatorType.Multiply)
            {
                return new Result(new Number(leftInt.Value * rightInt.Value));
            }
            if (kind == OperatorType.And)
            {
                return new Result(new Number(leftInt.Value != 0 && rightInt.Value != 0 ? 1 : 0));
            }
            if (kind == OperatorType.Or)
            {
                return new Result(new Number(leftInt.Value != 0 || rightInt.Value != 0 ? 1 : 0));
            }
            else if (kind == OperatorType.Divide)
            {
                if (rightInt.Value == 0)
                {
                    runtime.AddError(new RuntimeError($"Attempt to divide by 0 failed.", ctx));
                    return new Result();
                }
                else
                {
                    return new Result(new Number(leftInt.Value / rightInt.Value));
                }
            }
            else
            {
                runtime.AddError(new RuntimeError($"Operator not supported {kind}.", ctx));
                return new Result();
            }
        }
    }
}