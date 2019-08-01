using System;
using System.Collections.Generic;
using System.IO;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;
using Lopla.Language.Providers;

namespace Lopla.Starting
{
    public class MainHandler
    {
        public MainHandler(List<ILibrary> list)
        {
            Libs = list;
        }

        private List<ILibrary> Libs { get; }

        public void Main(string[] args)
        {
            var project = GetProject(args);

            if(project == null)
                return;
            
            var p = new Runner();
            var runtime = p.Run(project);

            if (runtime.HasErrors)
                Console.WriteLine(runtime.ToString());
        }

        public IProject GetProject(string[] args)
        {
            if (args != null && args.Length > 0) return new ProjectFromFolder(args[0], Libs);

            /*
                    https://www.microsoft.com/resources/documentation/windows/xp/all/proddocs/en-us/redirection.mspx
                    cat .\test.lp  | dotnet run --project Lopla.Console
                 */
            string stdin = null;
            if (Console.IsInputRedirected)
                using (var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
                {
                    stdin = reader.ReadToEnd();
                    return new MemoryScripts("Stdin", Libs, stdin);
                }

            return null;
        }
    }
}