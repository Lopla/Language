namespace Lopla.Tests
{
    using Language.Binary;
    using Language.Compiler.Mnemonics;
    using Language.Processing;
    using Xunit;

    public class AssignSpecs
    {
        [Fact]
        public void AllowsToAssignValueToStringLikeInArray()
        {
            var runtime = new Runtime(new Processors());

            runtime.StartRootScope(new Compilation("testScope"));

            //// set 1 on position 0
            var assignEmptyTable =
                new Assigment(null,
                    new VariableName(null, "testString"),
                    new ValueString(null, new String("some long string")));

            //// set 1 on position 0
            var sut =
                new Assigment(null,
                    new ValueTable(null,
                        new VariablePointer("testString"),
                        new ValueNumber(new Number(32))),
                    new ValueNumber(new Number('X')));

            assignEmptyTable.Execute(runtime);
            Assert.Empty(runtime.Errors);

            sut.Execute(runtime);

            var value = runtime.GetVariable("testString").Get(runtime) as String;

            runtime.EndRootScope();

            Assert.Empty(runtime.Errors);
            Assert.Equal("some long string                X", value?.Value);
        }

        [Fact]
        public void AllowsToAssignValueWhenTableIsEmpty()
        {
            var runtime = new Runtime(new Processors());

            runtime.StartRootScope(new Compilation("testScope"));

            //// set 1 on position 0
            var assignEmptyTable =
                new Assigment(null,
                    new VariableName(null, "testArray"),
                    new DeclareTable(null));

            //// set 1 on position 0
            var sut =
                new Assigment(null,
                    new ValueTable(null,
                        new VariablePointer("testArray"),
                        new ValueNumber(new Number(0))),
                    new ValueNumber(new Number(1)));

            assignEmptyTable.Execute(runtime);
            Assert.Empty(runtime.Errors);

            sut.Execute(runtime);

            runtime.EndRootScope();

            Assert.Empty(runtime.Errors);
        }
    }
}