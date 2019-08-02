using System.Collections.Generic;
using System.Linq;
using Lopla.Language.Interfaces;

namespace Lopla.Language.Environment
{
    public class GlobalScopes
    {
        private readonly IErrorHandler _errorHandler;
        private readonly Dictionary<string, GlobalScope> _globalScopes = new Dictionary<string, GlobalScope>();

        public GlobalScopes(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        public void Add(string name)
        {
            _globalScopes.Add(name, new GlobalScope(_errorHandler, name));
        }

        public void CreateFunctionScope(string name, string derivedScope)
        {
            var newScope = _globalScopes[name].DeriveFunctionScope(derivedScope);
            _globalScopes.Add(derivedScope, newScope);
        }

        public GlobalScope Get(string stackName)
        {
            return _globalScopes[stackName];
        }

        public IEnumerable<GlobalScope> GetAll()
        {
            return this._globalScopes.Select(e=>e.Value);
        }
    }
}