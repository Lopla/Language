namespace Lopla.Language.Processing
{
    using System;
    using System.Collections.Generic;
    using Binary;
    using Enviorment;
    using Errors;
    using Interfaces;

    public class Runtime : IErrorHandler
    {
        private readonly List<Error> _errors;
        private readonly Declarations _pro = new Declarations();
        private readonly Processors _processors;
        public readonly GlobalScopes StackWrapper;
        private string _currentStack;

        public Runtime(Processors processors)
        {
            _processors = processors;
            _processors.Init(this);
            _errors = new List<Error>();
            StackWrapper = new GlobalScopes(this);
        }

        public IEnumerable<Error> Errors => _errors;

        public bool ProcessingStpped()
        {
            return _processors.Get().IsStpped();
        }

        public Result EvaluateCodeBlock(IArgument argument)
        {
            if (argument is Mnemonic exp)
                return EvaluateCodeBlock(exp);

            AddError(new RuntimeError("Failed to evaluate argument.", null));

            return new Result();
        }

        private GlobalScope AddRootScope(string name)
        {
            StackWrapper.Add(name);
            _currentStack = name;
            return StackWrapper.Get(name);
        }

        public Result EvaluateCodeBlock(Mnemonic mnemonic)
        {
            return _processors.Get().Evaluate(mnemonic);
        }

        public Result EvaluateFunction(MethodPointer pointer, List<Result> methodParamters)
        {
            var args = _pro.GetArguments(pointer, methodParamters, this);
            var stackName = _pro.GetScope(pointer, this);
            if (!string.IsNullOrWhiteSpace(stackName))
            {
                var code = _pro.GetCode(pointer, this);

                _processors.Begin(StackWrapper.Get(stackName));
                var result = _processors.Get().EvaluateFunctionInScope(code, args, pointer);
                _processors.End();

                return result;
            }

            return new Result();
        }

        public void AddError(Error e)
        {
            _errors.Add(e);
            if (e is RuntimeError) Stop();
        }

        public void AddError(RuntimeError e)
        {
            var stackName = $"{_processors?.Get()?.RootStackName()}";
            e.Text = $"{stackName}\t{e.Text}";
            
            this.AddError((Error)e);
        }

        public void Stop()
        {
            _processors.Stop();
        }

        public Result GetVariable(string name)
        {
            return _processors.Get().GetVariable(name, this);
        }

        public void SetVariable(string variableName, Result functionParamter, bool coverUpVariable = false)
        {
            _processors.Get().SetVariable(variableName, functionParamter, coverUpVariable);
        }

        public void Register(MethodPointer methodName, Method body)
        {
            var mName = $"{methodName.NameSpace}.{methodName.Name}";
            //var name = _processors.Get().RootStackName();
            var stackName = _currentStack;
            _pro.Register(methodName, body, this, mName);
            
            this.StackWrapper.AddDerivedScope(stackName, mName);
        }

        public void FunctionReturn(Result result)
        {
            _processors.Get().Stop();
        }

        public Result EvaluateCodeBlock(List<Mnemonic> argumentsArgs)
        {
            var p = _processors.Get();
            return p.EvaluateWithinBlock(argumentsArgs, "block");
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

        public void Run(Compilation binary)
        {
            StartRootScope(binary);
            try
            {
                _processors.Get().Evaluate(binary.Mnemonics);
            }catch(Exception exception)
            {
                AddError(new RuntimeError(exception.Message));
            }
            EndRootScope();
        }

        public void StartRootScope(Compilation binary)
        {
            var stack = AddRootScope($"{binary.Name}");
            _processors.Begin(stack);
        }

        public void EndRootScope()
        {
            _processors.End();
        }
    }
}