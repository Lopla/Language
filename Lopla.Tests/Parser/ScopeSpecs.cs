using System.Collections.Generic;
using System.Linq;
using Lopla.Tests.Logic;
using Xunit;
using Xunit.Abstractions;

namespace Lopla.Tests.Parser
{
    public class ScopeSpecs : ParserTestBase
    {
        public static IEnumerable<object[]> ScopeTestCases()
        {
            var scripts = new List<List<object>>();

            scripts.Add(new List<object>
            {
                @"a=1
if (a==1) {
    a=2
    b=5
    Test.Write(b)
}
",
                "5"
            });
            scripts.Add(new List<object>
            {
                @"a=1
b=2
if (a==1) {
    b=5
    Test.Write(b)
}
Test.Write(b)
",
                "5",
                "5"
            });
            scripts.Add(new List<object>
            {
                @"a=1
b=2
while (a > 0) {
    if(b==2){
        b=5
        Test.Write(b)
    }
    a=0
}
Test.Write(b)
",
                "5",
                "5"
            });
            scripts.Add(new List<object>
            {
                @"a=1
b=2
Test.Write(b)

function tt.ok(ba){
    Test.Write(ba)
}

tt.ok(b)

b=4
Test.Write(b)

",
                "2",
                "2",
                "4"
            });
            scripts.Add(new List<object>
            {
                @"
/* recurrent method*/

function tt.ok(k, level){
    result=""""
    while(k > 0){
        /*(tt.ok(k, level+1)*/
        result = result + Test.ToString(k)
        k= k - 1
    }
    Test.Write(result)
}

tt.ok(5, 0)

",
                "54321",
                "2",
                "3"
            });

            return scripts.Select(e => e.ToArray());
        }

        [Theory]
        [MemberData(nameof(ScopeTestCases))]
        public void Scopes(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        public ScopeSpecs(ITestOutputHelper logger) : base(logger)
        {

        }
    }
}