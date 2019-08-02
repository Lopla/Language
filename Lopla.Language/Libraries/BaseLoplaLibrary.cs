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

        public virtual string Name => GetType().Name;

        public virtual Result Call(DoHandler action, Mnemonic context, Runtime runtime)
        {
            return action(context, runtime);
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
                NameSpace = this.Name
            }, m));
        }
        
        protected T GetArgument<T>(string label, Runtime runtime)
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