using Lopla.Language.Enviorment;

namespace Lopla.Language.Errors
{
    public class CompilationError : Error
    {
        public CompilationError(string text) : base(text)
        {
        }
    }
}