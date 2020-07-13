using System;
using System.Collections.Generic;
using System.Linq;
using Lopla.Language.Binary;
using Lopla.Language.Compiler.Mnemonics;
using Lopla.Language.Environment;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;

namespace Lopla.Language.Processing
{
    public class Processor
    {
        private readonly IRuntime _runtime;
        private readonly GlobalScope _stack;
        private bool _requestForStop;

        public Processor(IRuntime runtime, GlobalScope stack)
        {
            _runtime = runtime;
            _stack = stack;
        }

        public Result GetVariable(string name, IRuntime runtime)
        {
            return _stack.GetVariable(name, runtime);
        }

        public void SetVariable(string variableName, IValue functionParamter, bool coverUpVariable)
        {
            _stack.SetVariable(variableName, functionParamter, coverUpVariable);
        }

        public void Stop()
        {
            _requestForStop = true;
        }

        public Result Evaluate(Mnemonic mnemonic)
        {
            if (_requestForStop)
                return new Result();

            Result result = null;

            try
            {
                result = OnMnemonicExecute(mnemonic, _runtime); 
            }
            catch (Exception e)
            {
                _runtime.AddError(new RuntimeError(e.Message, mnemonic));
            }

            return result;
        }

        public virtual Result OnMnemonicExecute(Mnemonic mnemonic, IRuntime runtime)
        {
            return mnemonic.Execute(runtime);
        }

        public Result Evaluate(List<Mnemonic> mnemonics)
        {
            if (_requestForStop)
                return new Result();
            Result result = null;
            //// call all lines in method or file
            foreach (var mExpression in mnemonics)
            {
                if (_requestForStop)
                    break;


                result = _runtime.EvaluateCodeBlock(mExpression);
            }

            return result;
        }

        public Result EvaluateWithinBlock(List<Mnemonic> mnemonics, string block)
        {
            if (_requestForStop)
                return new Result();

            _stack.StartScope(block);
            Result r = this.Evaluate(mnemonics);
            _stack.EndScope();
            return r;
        }

        public Result Evaluate(Compilation binary, IRuntime runtime)
        {
            try
            {
                return Evaluate(binary.Mnemonics.Where(m => ! (m is MethodDeclaration ||
                                                            m is Assigment)).ToList());
            }
            catch (Exception exception)
            {
                runtime.AddError(new RuntimeError(exception.Message));
            }

            return new Result();
        }

        public void EvaluateMethodRegistrations(Compilation binary, IRuntime runtime)
        {
            try
            {
                Evaluate(
                    binary.Mnemonics.Where(m => m is MethodDeclaration ||
                                                m is Assigment)
                        .ToList());
            }
            catch (Exception exception)
            {
                runtime.AddError(new RuntimeError(exception.Message));
            }
        }

        public Result EvaluateFunctionInScope(List<Mnemonic> mCode,
            Dictionary<string, Result> args,
            MethodPointer pointer, Runtime runtime)
        {
            if (_requestForStop)
                return new Result();

            //// local function context
            //// prevents leak of variables in global scope 
            _stack.StartScope($"{pointer.NameSpace}.{pointer.Name}.{Guid.NewGuid()}");

            foreach (var result in args) _runtime.SetVariable(result.Key, 
                result.Value.Get(runtime), true);

            var r = Evaluate(mCode);

            _stack.EndScope();
            return r;
        }

        public string RootStackName()
        {
            return _stack.GetBoottomScope().Name;
        }

        internal IValue GetReference(string name, Runtime runtime)
        {
            return _stack.GetReference(name, runtime);
        }

        public bool IsStpped()
        {
            return _requestForStop;
        }

        public GlobalScope RootStack()
        {
            return _stack;
        }
    }
}