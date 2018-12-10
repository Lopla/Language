using System.Collections.Generic;
using Lopla.Language.Binary;
using Lopla.Language.Errors;

namespace Lopla.Language.Compiler
{
    public class CompilationResult
    {
        public readonly List<Error> Errors = new List<Error>();
        public Compilation Compilate;
    }
}