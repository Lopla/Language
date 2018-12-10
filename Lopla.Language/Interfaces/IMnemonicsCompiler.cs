using System.Collections.Generic;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Errors;

namespace Lopla.Language.Interfaces
{
    public interface IMnemonicsCompiler
    {
        Mnemonic Get(ASTNode? node);
        void AddError(CompilationError p0);
        List<CompilationError> Errors { get; }
    }
}