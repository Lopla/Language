namespace Lopla.Language.Errors
{
    using Binary;

    public class RuntimeError : Error
    {
        public RuntimeError(string text, Mnemonic context = null) 
            : base($"{context}\t{text}")
        {
        }
    }
}