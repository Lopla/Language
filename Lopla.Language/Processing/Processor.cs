using System;
using System.Collections.Generic;
using Lopla.Language.Binary;
using Lopla.Language.Enviorment;
using Lopla.Language.Errors;

namespace Lopla.Language.Processing
{
    public class Processor
    {
        private readonly Runtime _runtime;
        private readonly GlobalScope _stack;
        private bool _requestForStop;

        public Processor(Runtime runtime, GlobalScope stack)
        {
            _runtime = runtime;
            _stack = stack;
        }

        public Result GetVariable(string name, Runtime runtime)
        {
            return _stack.GetVariable(name, runtime);
        }

        public void SetVariable(string variableName, Result functionParamter, bool coverUpVariable)
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

        public virtual Result OnMnemonicExecute(Mnemonic mnemonic, Runtime runtime)
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

        public Result EvaluateFunctionInScope(List<Mnemonic> mCode,
            Dictionary<string, Result> args,
            MethodPointer pointer)
        {
            if (_requestForStop)
                return new Result();

            //// local function context
            //// orevents leak of variables in global scope 
            _stack.StartScope($"{pointer.NameSpace}.{pointer.Name}");

            foreach (var result in args) _runtime.SetVariable(result.Key, result.Value, true);

            var r = Evaluate(mCode);

            _stack.EndScope();
            return r;
        }

        public string RootStackName()
        {
            return _stack.GetBoottomScope().Name;
        }

        public bool IsStpped()
        {
            return _requestForStop;
        }
    }
}