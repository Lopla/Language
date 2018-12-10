namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Libraries;
    using Processing;

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
            if (_method != null)
                return _method.Do(this, runtime);

            return _action(this, runtime);
        }
    }
}