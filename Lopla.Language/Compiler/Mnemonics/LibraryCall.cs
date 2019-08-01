using System;
using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class LibraryCall : Mnemonic
    {
        private readonly ILibrary _library;
        private readonly DoHandler _action;

        public LibraryCall(ILibrary library, DoHandler action) : base(null)
        {
            _library = library;
            _action = action;
        }

        public override Result Execute(Runtime runtime)
        {
            Result result = null;
            try
            {
                result = _library.Call(_action, this, runtime);
            }
            catch (Exception e)
            {
                runtime.AddError(new RuntimeError(e.Message));
            }

            return result;
        }
    }
}