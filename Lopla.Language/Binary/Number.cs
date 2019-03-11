using System;

namespace Lopla.Language.Binary
{
    public class Number : IArgument, IValue<decimal>
    {
        public Number()
        {
        }

        public Number(decimal val)
        {
            Value = val;
        }

        public int ValueAsInt => Convert.ToInt32(Value);

        public byte ValueAsByte => Convert.ToByte(Value);

        public bool ValueAsBool => Convert.ToInt32(Value) == 1;

        public decimal Value { get; set; }

        public IValue Clone()
        {
            return new Number(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}