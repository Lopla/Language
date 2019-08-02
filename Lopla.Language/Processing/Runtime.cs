using System;
using System.Collections.Generic;
using Lopla.Language.Binary;
using Lopla.Language.Environment;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;

namespace Lopla.Language.Processing
{
    public class Runtime : IErrorHandler
    {
        private readonly List<Error> _errors;
        private readonly Declarations _declarations = new Declarations();
        private readonly Processors _processors;
        private readonly GlobalScopes _scopes;

        public Runtime(Processors processors)
        {
            _processors = processors;
            _processors.Init(this);
            _errors = new List<Error>();
            _scopes = new GlobalScopes(this);
        }

        #region Error handler
        public IEnumerable<Error> Errors => _errors;

        public void AddError(Error e)
        {
            _errors.Add(e);
            if (e is RuntimeError) Stop();
        }

        public void AddError(RuntimeError e)
        {
            var stackName = $"{_processors?.Get()?.RootStackName()}";
            e.Text = $"{stackName}\t{e.Text}";

            AddError((Error) e);
        }
        #endregion

        public void Evaluate(Compilation binary)
        {
            StartRootScope(binary);
            var result = _processors.Get().Evaluate(binary, this);
            EndRootScope();
        }

        public void Stop()
        {
            _processors.Stop();
        }

        public bool ProcessingStopped()
        {
            return _processors.Get().IsStpped();
        }

        #region scope managment
        public void StartRootScope(Compilation binary)
        {
            var stack = AddRootScope($"{binary.Name}");
            _processors.Begin(stack);
        }

        public void EndRootScope()
        {
            _processors.End();
        }

        private GlobalScope AddRootScope(string name)
        {
            _scopes.Add(name);
            return _scopes.Get(name);
        }
        #endregion

        #region Mnemonics

        public Result EvaluateCodeBlock(IArgument argument)
        {
            if (argument is Mnemonic exp)
                return EvaluateCodeBlock(exp);

            AddError(new RuntimeError("Failed to evaluate argument."));

            return new Result();
        }

        public Result EvaluateCodeBlock(Mnemonic mnemonic)
        {
            return _processors.Get().Evaluate(mnemonic);
        }

        public Result EvaluateMethodCall(MethodPointer pointer, List<Result> methodParameters)
        {
            var args = _declarations.GetArguments(pointer, methodParameters, this);
            var stackName = _declarations.GetScope(pointer, this);
            var derivedScopeName = stackName + Guid.NewGuid();
            _scopes.CreateFunctionScope(stackName, derivedScopeName);

            if (!string.IsNullOrWhiteSpace(stackName))
            {
                var code = _declarations.GetCode(pointer, this);

                _processors.Begin(_scopes.Get(derivedScopeName));
                var result = _processors.Get().EvaluateFunctionInScope(code, args, pointer);
                _processors.End();

                return result;
            }

            return new Result();
        }

        public Result EvaluateBlock(List<Mnemonic> argumentsArgs)
        {
            var p = _processors.Get();
            return p.EvaluateWithinBlock(argumentsArgs, "block");
        }

        public void FunctionReturn(Result result)
        {
            _processors.Get().Stop();
        }

        public IEnumerable<KeyValuePair<string, List<string>>> GetRegisteredMethods()
        {
            return _declarations.GetMethods();
        }
        #endregion

        #region memory access
        public Result GetVariable(string name)
        {
            return _processors.Get().GetVariable(name, this);
        }

        public void SetVariable(string variableName, Result functionParamter, bool coverUpVariable = false)
        {
            _processors.Get().SetVariable(variableName, functionParamter, coverUpVariable);
        }
        #endregion

        #region declaration and linking

        public void Register(MethodPointer methodName, Method body)
        {
            var name = _processors.Get().RootStackName();
            _declarations.Register(methodName, body, this, name);
        }

        public void Link(ILibrary library)
        {
            var scopeName = "library-" + library.Name;
            //// add scopes for libraries
            var global = AddRootScope(scopeName);
            _processors.Begin(global);

            foreach (var keyValuePair in library.Methods())
                Register(keyValuePair.Key, keyValuePair.Value);

            _processors.End();
        }

        #endregion

    }
}