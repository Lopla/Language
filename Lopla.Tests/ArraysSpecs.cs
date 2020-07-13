namespace Lopla.Tests
{
    using Language.Binary;
    using Language.Compiler.Mnemonics;
    using Language.Processing;
    using Xunit;

    public class ArraySpecs
    {
        [Fact]
        public void AllowsToReadValue()
        {
            var runtime = new Runtime(new Processors());

            runtime.StartRootScope(new Compilation("testScope"));


            var assignEmptyTable =
                new Assigment(null,
                    new VariableName(null, "testString"),
                    new DeclareTable(null));
            assignEmptyTable.Execute(runtime);
            Assert.Empty(runtime.Errors);

            for (int k = 0; k < 1024 * 10; k++)
            {
                var setArrayElementValue =
                    new Assigment(null,
                        new ValueTable(null,
                            new VariablePointer("testString"),
                            new ValueNumber(new Number(k))),
                        new ValueNumber(new Number(k)));

                setArrayElementValue.Execute(runtime);
            }

            Assert.Empty(runtime.Errors);

            //// read some element in table
            var getNthValue=
                new Assigment(
                        null,
                        new VariableName(null, "tempNthValue"),
                        new ValueTable(null, 
                            new VariablePointer("testString"), 
                            new ValueNumber(new Number(10)))
                    );
            getNthValue.Execute(runtime);
            Assert.Empty(runtime.Errors);
            
            var value = runtime.GetVariable("tempNthValue").Get(runtime) as Number;
            
            Assert.Empty(runtime.Errors);

            runtime.EndRootScope();

            Assert.Empty(runtime.Errors);
            Assert.Equal(10, value?.Value);
        }
   }
}