using Lopla.Tests.Logic;
using Xunit;

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
    }
}
