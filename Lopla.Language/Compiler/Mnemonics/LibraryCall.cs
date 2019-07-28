using System;
using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class LibraryCall : Mnemonic
    {
        private readonly DoHandler _action;
        private readonly LibraryMethod _method;

        public LibraryCall(DoHandler action) : base(null)
        {
            _action = action;
        }

        public LibraryCall(LibraryMethod action) : base(null)
        {
            _method = action;
        }

        public override Result Execute(Runtime runtime)
        {
            Result result = null;
            try
            {
                if (_method != null)
                    result = _method.Do(this, runtime);
                else
                    result = _action(this, runtime);
            }
            catch (Exception e)
            {
                runtime.AddError(new RuntimeError(e.Message));
            }

            return result;
        }
    }
}