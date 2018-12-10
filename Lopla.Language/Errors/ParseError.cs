using Lopla.Language.Enviorment;

namespace Lopla.Language.Errors
{
    public class ParseError : Error
    {
        public ParseError(string text) : base(text)
        {
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}:{System.Environment.NewLine}{Text}";
        }
    }
}