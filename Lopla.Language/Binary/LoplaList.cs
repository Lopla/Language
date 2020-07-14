namespace Lopla.Language.Binary
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class LoplaList : IArgument, IValue, ILoplaIndexedValue, IEnumerable<IValue>
    {
        private readonly List<IValue> _values = new List<IValue>();

        public LoplaList(params IValue[] list)
        {
            var k = 0;

            list?.ToList().ForEach(e => Set(k++, e));
        }

        public int Length => _values.Count;

        public IValue Clone()
        {
            var ll = new LoplaList();
            foreach (var result in _values) ll._values.Add(result.Clone());
            return ll;
        }

        public IValue Get(int idx)
        {
            if (_values.Count > idx)
            {
                return _values[idx];
            }

            return null;
        }

        int ILoplaIndexedValue.Length()
        {
            return this._values.Count;
        }

        public void Set(int i, IValue evaluate)
        {
            while (_values.Count < i + 1)
                _values.Add(null);
            _values[i] = evaluate;
        }

        public void Add(IValue result)
        {
            _values.Add(result);
        }

        public IEnumerator<IValue> GetEnumerator()
        {
            return this._values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}