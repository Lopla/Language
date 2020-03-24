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
b=4

Test.Write(b)
function tt.ok(ba){
    Test.Write(ba)
}
tt.ok(b)
Test.Write(b)
",
                "4",
                "4",
                "4"
            });
            scripts.Add(new List<object>
            {
                @"

function tt.ok(k, result){
    result=""""
    m = 0
    while(m < k){
        m = m + 1
        result = result + ""*""
    }
    return result
}

r = tt.ok(5, "" - "")
Test.Write(r)

",
                "*****"
            });
            scripts.Add(new List<object>
            {
                @"

function TT.Mul(a, b){
    r = 0
    if(b == 0){
        return 0
    }
    if(b > 0)
    {
        r = TT.Mul(a, b-1) + a
    }

    return r
}

r = TT.Mul(4,5)
Test.Write(r)

",
                "20"
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