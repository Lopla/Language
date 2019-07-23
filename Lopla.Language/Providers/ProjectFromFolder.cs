namespace Lopla.Language.Providers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Compiler;
    using Interfaces;

    public class ProjectFromFolder : IProject
    {
        private readonly List<string> _files;
        private readonly IEnumerable<ILibrary> _libs;
        private readonly List<IMedia> _media = new List<IMedia>();

        public ProjectFromFolder(
            string folderName,
            IEnumerable<ILibrary> libs)
        {
            if (File.Exists(folderName))
            {
                folderName = new FileInfo(folderName).DirectoryName;
            }

            var info = new DirectoryInfo(folderName);
            Name = info.Name;

            _files = new List<string>();
            _files.AddRange(
                Directory.GetFiles(folderName, "*.lpc", SearchOption.AllDirectories));
            _libs = libs;

            _media.AddRange(
                Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories)
                    .Select(f => new MediaFile(f)));
        }

        public ProjectFromFolder(
            List<string> fileNames,
            IEnumerable<ILibrary> libs)
        {
            _files = fileNames;
            _libs = libs;
        }

        public string Name { get; }

        public IEnumerable<Script> Scripts()
        {
            foreach (var file in _files)
            {
                var script = new Script
                {
                    Content = File.ReadAllText(file),
                    Name = Path.GetFileName(file)
                };
                yield return script;
            }
        }

        public IEnumerable<ILibrary> Libs()
        {
            return _libs;
        }

        public IEnumerable<IMedia> Media()
        {
            return _media;
        }
    }
}