namespace Lopla.Language.Binary
{
    using System.Text;

    public class String : IArgument, IValue<string>
    {
        public string Value { get; set; }

        public String()
        {

        }
        public String(string val)
        {
            this.Value = val;
        }

        public override string ToString()
        {
            return Value;
        }

        public IValue Clone()
        {
            return new String()
            {
                Value = new StringBuilder(this.Value).ToString()
            };
        }
    }
}