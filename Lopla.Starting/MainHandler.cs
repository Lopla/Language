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

        /// <summary>
        ///     expect folder where lopla project is stored (args[0], if not provided then searches for data in stdin
        ///     if that fails just quits
        /// </summary>
        /// <param name="args"></param>
        public void Main(string[] args)
        {
            var project = GetProject(args);

            if (project == null)
                return;

            var p = new Runner();
            var runtime = p.Run(project);

            if (runtime.HasErrors)
                Console.WriteLine(runtime.ToString());
        }

        public IProject GetProject(string[] args)
        {
            if (args != null && args.Length > 0) return new ProjectFromFolder(args[0], Libs);

            
            //// https://www.microsoft.com/resources/documentation/windows/xp/all/proddocs/en-us/redirection.mspx
            //// cat .\test.lp  | dotnet run --project Lopla.Console
            if (Console.IsInputRedirected)
                using (var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
                {
                    var stdin = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(stdin))
                        return null;
                    return new MemoryScripts("Stdin", Libs, stdin);
                }

            return null;
        }
    }
}