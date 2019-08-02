using Lopla.Tests.Logic;
using Xunit;
using Xunit.Abstractions;

namespace Lopla.Tests.Parser
{
    public class OrderOfCalcualtions : ParserTestBase
    {
        [Theory]
        [InlineData(@"a=1+2*3+4
Test.Write(a)
", "11")]
        public void MultiplicationOverAdd(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        public OrderOfCalcualtions(ITestOutputHelper logger) : base(logger)
        {
        }
    }
}
