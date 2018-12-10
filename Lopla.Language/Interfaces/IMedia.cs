namespace Lopla.Language.Interfaces
{
    using System.IO;

    public interface IMedia
    {
        Stream GetData();

        bool Match(string folder, string file);
    }
}