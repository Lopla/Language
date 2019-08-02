using Lopla.Tests.Logic;
using Xunit;
using Xunit.Abstractions;

namespace Lopla.Tests.Parser
{
    public class AssigmentsSpecs : ParserTestBase
    {
        [Theory]
        [InlineData(@"a = 3>2 && 1>0 
Test.Write(a)
", "1")]
        [InlineData(@"a = ( 1>0 )
Test.Write(a)
", "1")]
        public void AssignsAnd(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }


        [Theory]
        [InlineData(@"a=1
")]
        public void AssignsVariable(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"a = 1
Test.Write(a)
b = 2
a = b
Test.Write(a)
", "1", "2")]
        public void AssignsTwoVariables(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"a = 1
b = 2
a = b > 1
Test.Write(a)
", "1")]
        public void AssignsEvaluateGreater(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }
        [Theory]
        [InlineData(@"a = 0.1234
Test.Write(a)
", "0.1234")]
        public void AssignsRealValue(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"a =  -1
Test.Write(a)
", "-1")]
        public void AssignsValueSmallerThen0(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"a = -1 + -2 + 1 -1
Test.Write(a)
", "-3")]
        public void AssignsValueResultOfExpression(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"a = []
a[0]=1
a[10]=2
Test.Write(a[0])
Test.Write(a[10])
", "1", "2")]
        public void AssignsValueToATable(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        public AssigmentsSpecs(ITestOutputHelper logger) : base(logger)
        {
        }
    }
}