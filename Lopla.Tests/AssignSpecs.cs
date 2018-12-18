using System.Collections.Generic;
using Lopla.Language.Binary;
using Lopla.Language.Compiler.Mnemonics;
using Lopla.Language.Enviorment;
using Lopla.Language.Processing;
using Xunit;

namespace Lopla.Tests
{
    public class AssignSpecs
    {
        [Fact]
        public void AllowsToAssignValueWhenTableIsEmpty()
        {
            var sut = new Runtime(new Processors());

            sut.StartRootScope(new Compilation("a"));

            var variablePointer = new VariableName(null, "testArray");

            sut.Register(new MethodPointer
            {
                Name = "Method1",
                NameSpace = "Unittest"
            }, new Method
            {
                ArgumentList = new List<string>(),
                Code = new List<Mnemonic>()
                {
                    new Return(null, variablePointer)
                }
            });

            sut.EndRootScope();

            var assigment = new Assigment(null,
                variablePointer,
                new ValueTable());
                new ValueInteger(new Number(1)));
        }
    }
}