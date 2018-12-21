using System.Collections.Generic;
using System.Linq;
using Lopla.Tests.Logic;
using Xunit;

namespace Lopla.Tests.Parser
{
    public class MethodsSpecs : ParserTestBase
    {
        public static IEnumerable<object[]> MethodTestCases()
        {
            var scripts = new List<List<object>>();

            scripts.Add(new List<object>
            {
                @"a=[0,1]
Test.Write(a[1])
",
                "1"
            });
            scripts.Add(new List<object>
            {
                @"
function TestScript.Show(){
    Test.Write(""as"")
}

TestScript.Show()
",
                "as"
            });

            scripts.Add(new List<object>
            {
                @"

function TestScript.ShowA(){
    Test.Write(""showa"")
}

function TestScript.Show(){
    Test.Write(""show"")
    TestScript.ShowA()
}

TestScript.Show()
",
                "show",
                "showa"
            });
            scripts.Add(new List<object>
            {
                @"

text = ""123""
text1 = ""011""

function TestScript.Show(text, text1){
    Test.Write(text)
    Test.Write(text1)
}

TestScript.Show(""456"",""789"")

Test.Write(text)
Test.Write(text1)

",
                "456",
                "789",
                "123",
                "011"
            });
            return scripts.Select(e => e.ToArray());
        }

        [Theory]
        [MemberData(nameof(MethodTestCases))]
        public void Method(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }
         
        [Theory]
        [InlineData(@"a=[0,1]
a[1]=3
Test.Write(a[1])
", "3")]
        public void AssignedTableValuePassedToMethod(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"
function TestScript.Show(num){
    return num + 1 
}

Test.Write(TestScript.Show(455))
", "456")]
        public void ReturnFromMethodIsPossible(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }

        [Theory]
        [InlineData(@"
function TestScript.Show(num){
    i=0
    while(i < 10){
        if(i==3){
            return i
        }
        i=i+1
    }
    return num + 1 
}

Test.Write(TestScript.Show(3))
", "3")]
        public void ReturnFromWhileIsPossible(string script, params string[] args)
        {
            EvaluateCode(script, args);
        }
    }
}