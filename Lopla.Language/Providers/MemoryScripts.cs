namespace Lopla.Language.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Compiler;
    using Interfaces;

    public class MemoryScripts : IProject
    {
        private readonly List<ILibrary> _libs = new List<ILibrary>();
        public List<Script> Files = new List<Script>();

        public MemoryScripts(
            string name,
            IList<ILibrary> libs1, IEnumerable<string> code = null)
        {
            Name = name;
            _libs.AddRange(libs1);
            if (code != null)
            {
                var k = 0;
                foreach (var script in code)
                {
                    Files.Add(new Script
                    {
                        Content = script,
                        Name = "code-" + k
                    });
                    k++;
                }
            }
        }

        public MemoryScripts(
            string name,
            IList<ILibrary> libs1, string code)
            : this(name, libs1, new List<string>
            {
                code
            })
        {
            Name = name;
        }

        public MemoryScripts(string projName, List<ILibrary> toList, IEnumerable<Script> scripts)
        {
            Name = projName;
            _libs.AddRange(toList);

            Files.AddRange(scripts);
        }

        public string Name { get; }

        public IEnumerable<Script> Scripts()
        {
            return Files;
        }

        public IEnumerable<ILibrary> Libs()
        {
            return _libs;
        }

        public IEnumerable<IMedia> Media()
        {
            return null;
        }

        public void UpdateScript(string name, string code)
        {
            var s = Files.SingleOrDefault(n => n.Name == name);
            s.Content = code;
        }
    }
}