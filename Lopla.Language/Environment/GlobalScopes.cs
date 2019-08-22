using System.Collections.Generic;
using System.Linq;
using Lopla.Language.Interfaces;

namespace Lopla.Language.Environment
{
    public class GlobalScopes
    {
        private readonly IErrorHandler _errorHandler;
        private readonly List<GlobalScope> _globalScopes = new List<GlobalScope>();

        public GlobalScopes(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        public GlobalScope AddOrGetRootScope(string name)
        {
            var existing = _globalScopes.FirstOrDefault(g => g.FileName == name);
            if (existing != null)
            {
                return existing;
            }

            var newScope = new GlobalScope(_errorHandler, name);
            _globalScopes.Add(newScope);

            return newScope;
        }

        public GlobalScope CreateFunctionScope(GlobalScope scope)
        {
            var newScope = scope.DeriveFunctionScope();
            return newScope;
        }
    }
}