namespace Lopla.Language.Interfaces
{
    using Errors;

    public interface IErrorHandler
    {
        void AddError(Error e);

        void AddError(RuntimeError e);
    }
}