using System.Collections.Generic;
using System.Linq;
using Lopla.Language.Binary;
using Lopla.Language.Compiler.Mnemonics;
using Lopla.Language.Environment;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Libraries
{
    public delegate Result DoHandler(Mnemonic expression, Runtime runtime);

    public abstract class BaseLoplaLibrary : ILibrary
    {
        private readonly List<KeyValuePair<MethodPointer, Method>> _methods =
            new List<KeyValuePair<MethodPointer, Method>>();

        public IEnumerable<KeyValuePair<MethodPointer, Method>> Methods()
        {
            return _methods;
        }

        public string Name => this.GetType().FullName;

        protected void Add(string methodName, DoHandler action, params string[] arguments)
        {
            var m = new Method
            {
                Code = new List<Mnemonic>
                {
                    new LibraryCall(action)
                },
                ArgumentList = new List<string>()
            };
            if (arguments != null && arguments.Any())
                m.ArgumentList.AddRange(arguments.ToList());

            _methods.Add(new KeyValuePair<MethodPointer, Method>(new MethodPointer
            {
                Name = methodName,
                NameSpace = GetType().Name
            }, m));
        }

        protected void Add(string methodName, LibraryMethod action, params string[] arguments)
        {
            var m = new Method
            {
                Code = new List<Mnemonic>
                {
                    new LibraryCall(action)
                },
                ArgumentList = new List<string>()
            };
            if (arguments != null && arguments.Any())
                m.ArgumentList.AddRange(arguments.ToList());

            _methods.Add(new KeyValuePair<MethodPointer, Method>(new MethodPointer
            {
                Name = methodName,
                NameSpace = GetType().Name
            }, m));
        }

        protected Result CallFunction(string nameSpace, string function, Runtime runtime, params IValue[] arguments)
        {
            return runtime.EvaluateFunction(new MethodPointer
            {
                NameSpace = nameSpace,
                Name = function,
            }, new List<Result>(arguments.Select(d => new Result(d))));
        }

        protected T GetArgument<T>(string label, Runtime runtime)
            where T : class, IValue
        {
            if (runtime.GetVariable(label).Get(runtime) is T result)
            {
                return result;
            }
            else
            {
                runtime.AddError(new RuntimeError($"Argument {label} not provided or invalid type. Was expecting {typeof(T).Name}."));
            }
            return null;
        }
    }

    public class LibraryMethod
    {

        public virtual Result Do(Mnemonic expression, Runtime runtime)
        {
            return new Result();
        }
    }
}