namespace Lopla.Language.Errors
{
    public abstract class Error
    {
        public string Text;

        protected Error(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}: {Text}";
        }
    }
}