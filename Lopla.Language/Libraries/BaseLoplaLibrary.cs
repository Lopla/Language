namespace Lopla.Language.Libraries
{
    using System.Collections.Generic;
    using System.Linq;
    using Binary;
    using Compiler.Mnemonics;
    using Environment;
    using Errors;
    using Interfaces;

    public delegate Result DoHandler(Mnemonic expression, IRuntime runtime);

    public abstract class BaseLoplaLibrary : ILibrary
    {
        private readonly List<KeyValuePair<MethodPointer, Method>> _methods =
            new List<KeyValuePair<MethodPointer, Method>>();

        public IEnumerable<KeyValuePair<MethodPointer, Method>> Methods()
        {
            return _methods;
        }

        public virtual string Name => GetType().Name;

        public virtual Result Call(DoHandler action, Mnemonic context, IRuntime runtime)
        {
            return action(context, runtime);
        }

        protected Result CallFunction(string nameSpace, string function, IRuntime runtime, params IValue[] arguments)
        {
            return runtime.EvaluateMethodCall(new MethodPointer
            {
                NameSpace = nameSpace,
                Name = function,
            }, new List<Result>(arguments.Select(d => new Result(d))));
        }

        protected void Add(string methodName, DoHandler action, params string[] arguments)
        {
            var m = new Method
            {
                Code = new List<Mnemonic>
                {
                    new LibraryCall(this, action)
                },
                ArgumentList = new List<string>()
            };
            if (arguments != null && arguments.Any())
                m.ArgumentList.AddRange(arguments.ToList());

            _methods.Add(new KeyValuePair<MethodPointer, Method>(new MethodPointer
            {
                Name = methodName,
                NameSpace = Name
            }, m));
        }

        protected T GetArgument<T>(string label, IRuntime runtime)
            where T : class, IValue
        {
            if (runtime.GetVariable(label).Get(runtime) is T result)
                return result;
            runtime.AddError(
                new RuntimeError($"Argument {label} not provided or invalid type. Was expecting {typeof(T).Name}."));
            return null;
        }
    }
}