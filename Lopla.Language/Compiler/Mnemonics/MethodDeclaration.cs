﻿using System.Collections.Generic;
using System.Linq;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Environment;
using Lopla.Language.Interfaces;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class MethodDeclaration : Mnemonic
    {
        public Method Code;
        public MethodPointer Pointer;

        public MethodDeclaration(ASTNode? node, IMnemonicsCompiler compiler) : base(node)
        {
            var nameSpace = node.GetChildAndAddError(0, "namespace", compiler)
                .GetChildAndAddError(0, "LITERAL", compiler)?.Value;
            var method_name = node.GetChildAndAddError(2, "method_name", compiler)
                .GetChildAndAddError(0, "LITERAL", compiler)?.Value;
            var body = node.GetChildAndAddError(node.Value.Children.Count - 1, "block_of_lopla", compiler);
            var code = compiler.Get(body);

            var method_parameters = node.GetChildAndAddError(4, "method_parameters", compiler, true);

            var methodArguments = new List<string>();
            if (method_parameters != null && method_parameters.Value.Children.Any())
                foreach (var valueChild in method_parameters.Value.Children)
                {
                    var name = valueChild.GetChildAndAddError(0, "LITERAL", compiler).Value.Value;
                    methodArguments.Add(name);
                }

            Pointer = new MethodPointer {Name = method_name, NameSpace = nameSpace};
            Code = new Method
            {
                ArgumentList = methodArguments,
                Code = new List<Mnemonic>
                {
                    code
                }
            };
        }

        public override Result Execute(IRuntime runtime)
        {
            runtime.Register(Pointer, Code);

            return new Result();
        }
    }
}