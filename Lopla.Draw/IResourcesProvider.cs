namespace Lopla.Draw
{
    using System.IO;

    public interface IStreamFromResource
    {
        Stream GetResourceStream(string folder, string name);
    }
}