using System.Collections.Generic;
using System.Linq;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class MethodCall : Mnemonic
    {
        public MethodCall(MethodPointer pointer, params IArgument[] args) : base(null)
        {
            Pointer = pointer;
            Arguments = new Arguments(args);
        }

        public MethodCall(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            var namespaceName = node.GetChildAndAddError(0, "namespace", runtime)
                .GetChildAndAddError(0, "LITERAL", runtime, false);

            var methodName = node.GetChildAndAddError(2, "method_name", runtime)
                .GetChildAndAddError(0, "LITERAL", runtime, false);
            var methodArguments = node.GetChildAndAddError(4, "method_arguments", runtime, true);

            if (namespaceName.HasValue && methodName.HasValue)
            {
                Pointer = new MethodPointer
                {
                    NameSpace = namespaceName.Value.Value,
                    Name = methodName.Value.Value
                };

                Arguments = new Arguments();

                if (methodArguments.HasValue)
                    for (var k = 0; k < methodArguments.Value.Children.Count; k += 2)
                    {
                        var element = methodArguments.Value.Children.ElementAt(k);
                        var value = runtime.Get(element);
                        Arguments.Args.Add(value);
                    }
            }
        }

        public Arguments Arguments { get; set; }

        public MethodPointer Pointer { get; set; }

        public override Result Execute(Runtime runtime)
        {
            var incomingParamters = Arguments.Args.ToList();
            var methodParamters = new List<Result>();

            foreach (var argument in incomingParamters)
            {
                var result = runtime.EvaluateCodeBlock(argument);
                if (result.HasResult())
                    methodParamters.Add(result);
                else
                    runtime.AddError(new RuntimeError($"Too much results or incorrect result {Pointer}", this));
            }

            return runtime.EvaluateFunction(Pointer, methodParamters);
        }
    }
}