using System;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;

namespace Lopla.Language.Compiler
{
    public class Compiler
    {
        private readonly IMnemonicsCompiler _compiler = new MnemonicsProvider();

        public CompilationResult Compile(ASTNode resultRoot, string name)
        {
            var result = new CompilationResult();
            result.Compilate = new Compilation(name);

            var symbol = resultRoot.Symbol.Name;
            if (symbol == "lopla")
                foreach (var resultRootChild in resultRoot.Children)
                    try
                    {
                        var compilation = _compiler.Get(resultRootChild);
                        _compiler.Errors.ForEach(result.Errors.Add);
                        result.Compilate.Add(compilation);
                    }
                    catch (Exception e)
                    {
                        result.Errors.Add(new CompilationError($"Critical compiler error {e.Message}"));
                    }
            else
                result.Errors.Add(new CompilationError($"Found unexpected node - {symbol} was expecting lopla."));

            return result;
        }
    }
}