using System;
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

        public GlobalScope Add(string name)
        {
            var newScope = new GlobalScope(_errorHandler, name);
            _globalScopes.Add(name, newScope);

            return newScope;
        }

        public GlobalScope CreateFunctionScope(GlobalScope scope)
        {
            var newScope = scope.DeriveFunctionScope();
            _globalScopes.Add(Guid.NewGuid().ToString(), newScope);
            return newScope;
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