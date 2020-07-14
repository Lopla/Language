namespace Lopla.Language.Binary
{
    using System;
    using System.Linq;
    using System.Text;
    using Interfaces;

    public class String : IArgument, IValue<string>, ILoplaIndexedValue
    {
        public String()
        {
        }

        public String(string val)
        {
            Value = val;
        }

        public string Value { get; set; }

        public IValue Clone()
        {
            return new String
            {
                Value = new StringBuilder(Value).ToString()
            };
        }

        public void Set(int idx, IValue newValue)
        {
            var stringAsList = Value.ToList();
            while (stringAsList.Count < idx + 1)
                stringAsList.Add(' ');

            var incomingValue = newValue;
            if (incomingValue is Number)
            {
                stringAsList[idx] = (char)(incomingValue as Number).Value;
                this.Value = new string(stringAsList.ToArray());
            }
            else
            {
                throw new Exception($"Failed to set indexed value in string. (expected {nameof(Number)} but received {incomingValue?.GetType().Name})");
            }
        }

        public IValue Get(int idx)
        {
            var c = (int) Value[idx];
            return new Number(c);
        }

        public int Length()
        {
            return this.Value.Length;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}