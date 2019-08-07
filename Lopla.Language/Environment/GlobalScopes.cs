using System.Collections.Generic;
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

        public GlobalScope Add(string name)
        {
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