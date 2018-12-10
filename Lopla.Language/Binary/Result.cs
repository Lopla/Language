namespace Lopla.Language.Binary
{
    using System.Collections.Generic;
    using System.Linq;
    using Errors;
    using Processing;

    /// <summary>
    /// This is a result (wrapper around entity)
    /// In theory ready for touple
    /// </summary>
    public class Result
    {
        private readonly List<IValue> _results = new List<IValue>();

        public Result()
        {
        }

        public Result(IValue value)
            : this(new List<IValue> {value})
        {
        }

        private Result(IEnumerable<IValue> values)
            : this()
        {
            foreach (var value in values) _results.Add(value.Clone());
        }

        public IValue Get(Runtime runtime)
        {
            if (_results.Count == 1)
                return _results[0].Clone();
            runtime.AddError(new RuntimeError($"Results count should be one 1 found {_results.Count}", null));

            return null;
        }

        public bool HasResult()
        {
            return _results.Count == 1;
        }

        public override string ToString()
        {
            return string.Join(",", _results.Select(r => r.ToString()));
        }

        public Result Clone()
        {
            return new Result(_results);
        }
    }
}