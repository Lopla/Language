namespace Lopla.Language.Providers
{
    using System;
    using System.IO;
    using Interfaces;

    public class MediaFile : IMedia
    {
        private readonly string _file;
        private readonly string _path;

        public MediaFile(string path)
        {
            _path = path;
            _file = Path.GetFileName(path);
        }

        public bool Match(string folder, string file)
        {
            return
                _file.Equals(file, StringComparison.OrdinalIgnoreCase) &&
                File.Exists(_path);
        }

        public Stream GetData()
        {
            return new FileStream(_path, FileMode.Open);
        }
    }
}