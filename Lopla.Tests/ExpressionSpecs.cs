using Lopla.Language.Binary;
using Lopla.Language.Compiler.Mnemonics;
using Lopla.Language.Processing;
using Xunit;

namespace Lopla.Tests
{
    public class ExpressionSpecs
    {
        [InlineData(12, 12, OperatorType.Equals, 1)]
        [InlineData(12, 4, OperatorType.GreaterThen, 1)]
        [InlineData(12, 4, OperatorType.GreaterThenOrEqual, 1)]
        [InlineData(12, 4, OperatorType.LessThen, 0)]
        [InlineData(4, 12, OperatorType.LessThenOrEqual, 1)]
        [InlineData(1, 0, OperatorType.NotEquals, 1)]
        [Theory]
        public void EvaluationOfBoelanLogic(int a, int b, OperatorType kind, int expected)
        {
            var r = Expression.ArgumentCalcualte<Number, decimal>
                (new Number(a), new Number(b), kind);
            Assert.True(r.HasResult(), "Incorrect arguments for this operation.");
            var resultValue = r.Get(new Runtime(new Processors())) as Number;
            
            Assert.Equal(resultValue?.Value, expected);
        }

        [InlineData(12, 12, OperatorType.Add, 24)]
        [InlineData(12, 12, OperatorType.Multiply, 144)]
        [InlineData(12, 12, OperatorType.Subtract, 0)]
        [InlineData(24, 12, OperatorType.Divide, 2)]
        [InlineData(12, 12, OperatorType.Equals, 1)]
        [InlineData(12, 4, OperatorType.GreaterThen, 1)]
        [InlineData(12, 4, OperatorType.GreaterThenOrEqual, 1)]
        [InlineData(12, 4, OperatorType.LessThen, 0)]
        [InlineData(4, 12, OperatorType.LessThenOrEqual, 1)]
        [InlineData(1, 0, OperatorType.And, 0)]
        [InlineData(1, 0, OperatorType.Or, 1)]
        [InlineData(1, 0, OperatorType.NotEquals, 1)]
        [Theory]
        public void EvaluationOfMathFormula(int a, int b, OperatorType kind, int expected)
        {
            var r = Expression.ArgumentCalculate
                (new Number(a), new Number(b), kind, new Runtime(new Processors()), new Nop(null));
            Assert.True(r.HasResult(), "Incorrect arguments for this operation.");
            var resultValue = r.Get(new Runtime(new Processors())) as Number;

            Assert.Equal(resultValue?.Value, expected);
        }
    }
}