using System.Collections.Generic;
using Lopla.Language.Binary;
using Lopla.Language.Environment;
using Lopla.Language.Errors;

namespace Lopla.Language.Interfaces
{
    public interface IRuntime : IErrorHandler
    {
        IEnumerable<Error> Errors { get; }
        void Evaluate(Compilation binary);
        void Stop();
        bool ProcessingStopped();
        void StartRootScope(Compilation binary);
        void EndRootScope();
        Result EvaluateCodeBlock(IArgument argument);
        Result EvaluateCodeBlock(Mnemonic mnemonic);
        Result EvaluateMethodCall(MethodPointer pointer, List<Result> methodParameters);
        Result EvaluateBlock(List<Mnemonic> argumentsArgs);
        void FunctionReturn(Result result);
        IEnumerable<KeyValuePair<string, List<string>>> GetRegisteredMethods();
        Result GetVariable(string name);
        void SetVariable(string variableName, IValue functionParamter, bool coverUpVariable = false);
        void Register(MethodPointer methodName, Method body);
        void Link(ILibrary library);
        IValue GetReference(string tablePointerName);
    }
}