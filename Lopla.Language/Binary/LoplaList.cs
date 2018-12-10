namespace Lopla.Language.Binary
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class LoplaList : IArgument, IValue, IEnumerable<Result>
    {
        private readonly List<Result> _values = new List<Result>();

        public LoplaList(params Result[] list)
        {
            var k = 0;

            list?.ToList().ForEach(e => Set(k++, e));
        }

        public int Length => _values.Count;

        public IEnumerator<Result> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IValue Clone()
        {
            var ll = new LoplaList();
            foreach (var result in _values) ll._values.Add(result.Clone());
            return ll;
        }

        public Result Get(Number idx)
        {
            if (_values.Count > idx.Value)
            {
                var r = _values[idx.ValueAsInt].Clone();
                return r;
            }

            return new Result();
        }

        public void Set(int i, Result evaluate)
        {
            while (_values.Count < i + 1)
                _values.Add(new Result());
            _values[i] = evaluate;
        }

        public void Add(Result result)
        {
            _values.Add(result);
        }
    }
}