using System.Collections.Generic;
using Lopla.Language.Binary;
using Lopla.Language.Compiler.Mnemonics;
using Lopla.Language.Enviorment;
using Lopla.Language.Processing;
using Xunit;

namespace Lopla.Tests
{
    using Language.Interfaces;

    public class AssignSpecs
    {
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
                        new ValueInteger(new Number(0))),
                    new ValueInteger(new Number(1)));

            assignEmptyTable.Execute(runtime);
            Assert.Empty(runtime.Errors);

            sut.Execute(runtime);

            runtime.EndRootScope();

            Assert.Empty(runtime.Errors);
        }
    }
}