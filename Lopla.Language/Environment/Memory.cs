using System.Collections.Generic;
using System.Linq;
using Lopla.Language.Binary;

namespace Lopla.Language.Environment
{
    public class Memory
    {
        private readonly Dictionary<MemoryPointer, Result> _data = 
            new Dictionary<MemoryPointer, Result>();

        public void Set(VariablePointer pointer, Result value)
        {
            var m = LocateByKey(pointer);

            if (m == null)
            {
                m = new MemoryPointer
                {
                    Name = pointer.Name
                };
                _data.Add(m, null);
            }

            _data[m] = value;
        }

        public Result Get(VariablePointer pointer)
        {
            var d = LocateByKey(pointer);

            if (d != null)
                return _data[d];

            return null;
        }

        private MemoryPointer LocateByKey(VariablePointer pointer)
        {
            var d = _data.Keys
                .FirstOrDefault(me => me.Name == pointer.Name);
            return d;
        }

        public override string ToString()
        {
            return $"{_data.Count}";
        }
    }
}