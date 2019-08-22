using System;
using System.Collections.Generic;
using Lopla.Language.Binary;
using Lopla.Language.Environment;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;

namespace Lopla.Language.Processing
{
    public class Runtime : IRuntime
    {
        private readonly Declarations _declarations = new Declarations();
        private readonly List<Error> _errors;
        private readonly Processors _processors;
        private readonly GlobalScopes _scopes;

        public Runtime(Processors processors)
        {
            _processors = processors;
            _processors.Init(this);
            _errors = new List<Error>();
            _scopes = new GlobalScopes(this);
        }

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
            return _scopes.Add(name);
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
            var stack = _declarations.GetScope(pointer, this);
            
            var derivedScope = _scopes.CreateFunctionScope(stack);

            var code = _declarations.GetCode(pointer, this);

            _processors.Begin(derivedScope);
            var result = _processors.Get().EvaluateFunctionInScope(code, args, pointer);
            _processors.End();

            return result;
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
            var stack = _processors.Get().RootStack();
            _declarations.Register(methodName, body, this, Guid.NewGuid().ToString(), stack);
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

        public void EvaluateFunctionDeclatrations(Compilation binary)
        {
            StartRootScope(binary);
            _processors.Get().EvaluateMethodRegistrations(binary, this);
            EndRootScope();
        }
    }
}