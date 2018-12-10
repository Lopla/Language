namespace Lopla.Language.Enviorment
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class GlobalScopes
    {
        private readonly IErrorHandler _errorHandler;
        private readonly Dictionary<string, GlobalScope> _stacks = new Dictionary<string, GlobalScope>();

        public GlobalScopes(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        public void Add(string name)
        {
            _stacks.Add(name, new GlobalScope(_errorHandler, name));
        }

        public void AddDerivedScope(string name, string derivedScope)
        {
            var newScope = _stacks[name].DeriveFunctionScope(derivedScope);
            _stacks.Add(derivedScope, newScope);
        }

        public GlobalScope Get(string stackName)
        {
            return _stacks[stackName];
        }

        public IEnumerable<GlobalScope> GetAll()
        {
            return this._stacks.Select(e=>e.Value);
        }
    }
}