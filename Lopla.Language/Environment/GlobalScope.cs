using System.Collections.Generic;
using System.Linq;
using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Environment
{
    public class GlobalScope
    {
        private readonly IErrorHandler _errorHandler;

        public GlobalScope(IErrorHandler errorHandler, string fileName)
        {
            _errorHandler = errorHandler;
            this.StartScope($"{fileName}");
        }

        private Stack<Scope> ScopesStack { get; } = new Stack<Scope>();

        public void EndScope()
        {
            ScopesStack.Pop();
        }

        public void StartScope(string name)
        {
            if (ScopesStack.Count > 1000)
                _errorHandler.AddError(new RuntimeError("Stack overflow", null));
            else
                ScopesStack.Push(new Scope
                {
                    Name = name
                });
        }

        public Scope GetBoottomScope()
        {
            return ScopesStack.ToArray().Last();
        }

        public void SetVariable(string variableName, Result functionParamter, bool coverUpVariable)
        {
            if (!coverUpVariable)
                foreach (var scope in ScopesStack)
                {
                    var val = scope.Mem.Get(new VariablePointer {Name = variableName});
                    if (val != null)
                    {
                        scope.Mem.Set(new VariablePointer {Name = variableName}, functionParamter);
                        return;
                    }
                }

            ScopesStack.Peek()
                .Mem
                .Set(new VariablePointer
                {
                    Name = variableName
                }, functionParamter);
        }

        public Result GetVariable(string name, Runtime runtime)
        {
            Result val = null;
            foreach (var scope in ScopesStack)
            {
                val = scope.Mem.Get(new VariablePointer
                {
                    Name = name
                });
                if (val != null) break;
            }

            if (val == null) runtime.AddError(new RuntimeError($"Value not defined {name}.", null));

            return val;
        }

        public GlobalScope DeriveFunctionScope(string scopName)
        {
            var s = new GlobalScope(_errorHandler, scopName);
            s.ScopesStack.Push(this.GetBoottomScope());
            return s;
        }
    }
}