using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Binary
{
    /// <summary>
    ///     This is a result (wrapper around entity)
    /// </summary>
    public class Result
    {
        private readonly IValue _result;

        public Result()
        {
        }

        public Result(IValue value)
        {
            _result = value?.Clone();
        }

        public IValue Get(IRuntime runtime)
        {
            if (_result != null)
                return _result.Clone();
            runtime.AddError(
                new RuntimeError("Result not provided", null));

            return null;
        }

        public bool HasResult()
        {
            return _result != null;
        }

        public override string ToString()
        {
            return $"{_result}";
        }

        public Result Clone()
        {
            return new Result(_result);
        }
    }
}