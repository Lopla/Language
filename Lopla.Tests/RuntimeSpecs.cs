using System.Collections.Generic;
using System.Linq;
using Lopla.Language.Binary;
using Lopla.Language.Compiler.Mnemonics;
using Lopla.Language.Environment;
using Lopla.Language.Processing;
using Lopla.Libs;
using Xunit;

namespace Lopla.Tests
{
    public class RuntimeSpecs
    {
        [Fact]
        public void AllowsToSetAndReadTheSameInteger()
        {
            var testValue = 1;

            var compilation = new Compilation("A");
            var runtime = new Runtime(new Processors());
            runtime.StartRootScope(compilation);
            runtime.SetVariable("name1", new Result(new Number(testValue)));

            var val = runtime.GetVariable("name1");
            decimal found = 0;
            if (val.Get(runtime) is Number i) found = i.Value;

            Assert.Equal(testValue, found);

            runtime.EndRootScope();
        }

        [Fact]
        public void AllowsToSetInUpperScopeAndReadTheSameInteger()
        {
            var testValue = 1;
            var compilation = new Compilation("A");
            var runtime = new Runtime(new Processors());
            runtime.StartRootScope(compilation);

            runtime.SetVariable("name1", new Result(new Number(testValue)));

            var val = runtime.GetVariable("name1");
            decimal found = 0;
            if (val.Get(runtime) is Number i) found = i.Value;

            Assert.Equal(testValue, found);
        }

        [Fact]
        public void DontChangeValueInUpperScope()
        {
            var testValue = 1;
            var compilation = new Compilation("A");
            var sut = new Runtime(new Processors());
            sut.StartRootScope(compilation);

            sut.Register(new MethodPointer
            {
                Name = "Method1",
                NameSpace = "Unittest"
            }, new Method
            {
                ArgumentList = new List<string>
                {
                    "argument1"
                },
                Code = new List<Mnemonic>()
            });

            sut.SetVariable("argument1", new Result(new Number(testValue)));

            //// not set in this scope
            var valBeforeCall = sut.GetVariable("argument1");
            Assert.Equal(testValue, ((Number) valBeforeCall.Get(sut)).Value);

            var methodCall = new MethodCall(
                new MethodPointer("Method1", "Unittest"),
                new ValueInteger(new Number(123)));

            //// we call method
            sut.EvaluateCodeBlock(methodCall);

            var valAfterCall = sut.GetVariable("argument1");
            Assert.Equal(testValue, ((Number) valBeforeCall.Get(sut)).Value);

            Assert.Empty(sut.Errors);
        }

        [Fact]
        public void MethodsDeclaresNewVariablesAndTheyAreNotAcceisbleByOtherMethods()
        {
            var variableValue_I_inMethodScope = 10;
            var variableName_I_inMethodScope = "globalvariable";

            var sut = new Runtime(new Processors());
            sut.StartRootScope(new Compilation("a"));

            var variablePointer = new VariableName(null, variableName_I_inMethodScope);

            var methodCall1 =
                new MethodCall(
                    new MethodPointer("Method1", "Unittest")
                );
            var methodCall2 =
                new MethodCall(
                    new MethodPointer("Method2", "Unittest")
                );

            sut.Register(new MethodPointer
            {
                Name = "Method1",
                NameSpace = "Unittest"
            }, new Method
            {
                ArgumentList = new List<string>(),
                Code = new List<Mnemonic>
                {
                    new Assigment(null,
                        variablePointer,
                        new ValueInteger(new Number(variableValue_I_inMethodScope + 1)))
                }
            });

            sut.Register(new MethodPointer
            {
                Name = "Method2",
                NameSpace = "Unittest"
            }, new Method
            {
                ArgumentList = new List<string>(),
                Code = new List<Mnemonic>
                {
                    new Assigment(null,
                        variablePointer,
                        new ValueInteger(new Number(variableValue_I_inMethodScope))),
                    methodCall1,
                    new Return(null, variablePointer)
                }
            });

            var r = sut.EvaluateCodeBlock(methodCall2);

            sut.EndRootScope();

            var value = r.Get(sut) as Number;
            Assert.Equal(variableValue_I_inMethodScope, value?.Value);
        }

        [Fact]
        public void MethodsHasValuesFromItsRootScopeWithoutAccessToVaraiblesFromOtherRootScopes()
        {
            var variableValue_I_inRootScope = 10;
            var variableName_I_inRootScope = "globalvariable";

            var sut = new Runtime(new Processors());

            //// setup method in scope b
            sut.StartRootScope(new Compilation("b"));

            var variablePointer = new VariableName(null, variableName_I_inRootScope);

            sut.Register(new MethodPointer
            {
                Name = "Method1",
                NameSpace = "Unittest"
            }, new Method
            {
                ArgumentList = new List<string>(),
                Code = new List<Mnemonic>
                {
                    new Return(null, variablePointer)
                }
            });

            sut.EndRootScope();

            //// set variable in scope a
            sut.StartRootScope(new Compilation("a"));
            sut.SetVariable(variableName_I_inRootScope, new Result(new Number(variableValue_I_inRootScope)));
            var methodCall =
                new MethodCall(
                    new MethodPointer("Method1", "Unittest")
                );

            //// we call method
            var result = sut.EvaluateCodeBlock(methodCall);

            sut.EndRootScope();

            Assert.Single(sut.Errors);
        }

        [Fact]
        public void ProtectsValuesInSubForMethodScopeBeforeRead()
        {
            var testValue = 1;
            var compilation = new Compilation("A");
            var sut = new Runtime(new Processors());
            sut.StartRootScope(compilation);

            sut.Register(new MethodPointer
            {
                Name = "Method1",
                NameSpace = "Unittest"
            }, new Method
            {
                ArgumentList = new List<string>
                {
                    "argument1"
                },
                Code = new List<Mnemonic>()
            });

            //// not set in this scope
            var valBeforeCall = sut.GetVariable("argument1");
            Assert.Null(valBeforeCall);

            var methodCall =
                new MethodCall(
                    new MethodPointer("Method1", "Unittest"),
                    new ValueInteger(new Number(testValue))
                );

            //// we call method
            sut.EvaluateCodeBlock(methodCall);

            var valAfterCall = sut.GetVariable("argument1");
            Assert.Null(valAfterCall);

            Assert.Equal(2, sut.Errors.Count());
        }

        [Fact]
        public void ResultsWithProperListOfLibs()
        {
            var compilation = new Compilation("libsUnitTest");
            var sut = new Runtime(new Processors());
            sut.Link(new Lp());

            var methods = sut.GetRegisteredMethods();
            Assert.NotEmpty(methods.ToList());
            sut.StartRootScope(compilation);
        }
    }
}