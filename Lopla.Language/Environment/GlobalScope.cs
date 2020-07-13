using System;
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
        public string FileName { get; }

        public GlobalScope(IErrorHandler errorHandler, string fileName)
        {
            _errorHandler = errorHandler;
            FileName = fileName;
            StartScope($"{fileName}");
        }

        /// <summary>
        ///     Used to create derived scope (with access to all root variables)
        /// </summary>
        /// <param name="errorHandler"></param>
        /// <param name="rootScope"></param>
        protected GlobalScope(IErrorHandler errorHandler, GlobalScope rootScope)
        {
            _errorHandler = errorHandler;
            ScopesStack.Push(rootScope.GetBoottomScope());
        }

        private Stack<Scope> ScopesStack { get; } = new Stack<Scope>();

        public void EndScope()
        {
            ScopesStack.Pop();
        }

        public void StartScope(string name = null)
        {
            if (ScopesStack.Count > 1000)
                _errorHandler.AddError(new RuntimeError("Stack overflow"));
            else
                ScopesStack.Push(new Scope
                {
                    Name = name ?? Guid.NewGuid().ToString()
                });
        }

        public Scope GetBoottomScope()
        {
            return ScopesStack.ToArray().Last();
        }

        /// <summary>
        ///     Allows to setup variable
        /// </summary>
        /// <param name="variableName">variable name</param>
        /// <param name="functionParamter">value</param>
        /// <param name="coverUpVariable">
        ///     if true then this variable will cover the one in upper scope (useful when setting up arguments for
        ///     setting up function arguments in method scope)
        /// </param>
        public void SetVariable(string variableName, IValue newValue, bool coverUpVariable)
        {
            if (!coverUpVariable)
                foreach (var scope in ScopesStack)
                {
                    var val = scope.Mem.Get(new VariablePointer {Name = variableName});
                    if (val != null)
                    {
                        scope.Mem.Set(new VariablePointer {Name = variableName}, newValue);
                        return;
                    }
                }

            ScopesStack
                .Peek()
                .Mem
                .Set(new VariablePointer
                {
                    Name = variableName
                }, newValue);
        }

        public Result GetVariable(string name, IRuntime runtime)
        {
            
            return new Result(this.GetReference(name, runtime));
        }

        public GlobalScope DeriveFunctionScope()
        {
            var gs = new GlobalScope(_errorHandler, this);
            gs.StartScope();
            return gs;
        }

        public override string ToString()
        {
            return $"#{ScopesStack.Count} lst: {string.Join(" ", ScopesStack?.Select(s => s?.ToString()))}";
        }

        public IValue GetReference(string name, IRuntime runtime)
        {
            IValue val = null;
            foreach (var scope in ScopesStack)
            {
                val = scope.Mem.Get(new VariablePointer
                {
                    Name = name
                });
                if (val != null) break;
            }

            if (val == null) runtime.AddError(new RuntimeError($"Value not defined {name}."));
            return val;
        }
    }
}