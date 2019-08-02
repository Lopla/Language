using System.Collections.Generic;
using System.Linq;
using Lopla.Tests.Logic;
using Xunit;
using Xunit.Abstractions;

namespace Lopla.Tests.Parser
{
    public class IfSpecs : ParserTestBase
    {
        private static IEnumerable<object[]> Scripts(List<List<object>> list)
        {
            return list.Select(e => e.ToArray());
        }

        public static IEnumerable<object[]> IfTestCases()
        {
            var scripts = new List<List<object>>
            {
                new List<object>
                {
                    @"a=3
if (a==3) {
    Test.Write(""dwa"")
}
",
                    "dwa"
                },
                new List<object>
                {
                    @"a=3
if (a==2) {
    Test.Write(""one"")
}
"
                },
                new List<object>
                {
                    @"a=4
b=5
if (a==4) {
    Test.Write(""one"")
    if (b==5) {
        Test.Write(""two"")
    }
    a=5
    if (b==a) {
        Test.Write(""three"")
    }
}
",
                    "one",
                    "two",
                    "three"
                }
            };

            return Scripts(scripts);
        }

        [Theory]
        [MemberData(nameof(IfTestCases))]
        public void If(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"
a=1 && 1

Test.Write(a)
", 
            "1")]
        public void IfAndTrue(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"
a=1 && 0

Test.Write(a)
",
            "0")]
        public void IfAndFalse(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"
a=2 || 0

Test.Write(a)
",
            "1")]
        public void IfOrTrue(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"
a=0 || 0

Test.Write(a)
",
            "0")]
        public void IfOrFalse(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }


        [Theory]
        [InlineData(@"a=3
if (a==3)
{
    Test.Write(""one"")
}
",
            "one")]
        public void MultiLineIfDeclarationIsOk(string script, params string[] args)
        {
            EvaluateCode(script, args);
        } 
        
        [Theory]
        [InlineData(@"a=3
b=-1
if ((a==3) && (b == -1) )
{
    Test.Write(""one"")
}
",
            "one")]
        public void AllowsToSpecifyAndExpression(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        public IfSpecs(ITestOutputHelper logger) : base(logger)
        {
        }
    }
}