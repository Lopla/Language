using System.Collections.Generic;
using System.IO;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;
using Lopla.Language.Providers;
using Lopla.Libs;

namespace Lopla.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                RunProject(args);                
            }
            else
            {
                /*
                    https://www.microsoft.com/resources/documentation/windows/xp/all/proddocs/en-us/redirection.mspx
                    cat .\test.lp  | dotnet run --project Lopla.Console
                 */
                string stdin = null;
                if (System.Console.IsInputRedirected)
                {
                    using (StreamReader reader = new StreamReader(System.Console.OpenStandardInput(), System.Console.InputEncoding))
                    {
                        stdin = reader.ReadToEnd();
                        RunProject(stdin);
                    }
                }
            }
        }

        private static void RunProject(string text)
        {
            //// project is poor 
            //// only few dll's
            var p = new Runner();
            var runtime = p.Run(
                
                new MemoryScripts("Stdin",
                    new List<ILibrary>
                    {
                        new IO(),
                        new Lp()
                    },text));
            if (runtime.HasErrors)
                System.Console.WriteLine(runtime.ToString());
        }

        private static void RunProject(string[] args)
        {
            var p = new Runner();
            var runtime = p.Run(
                new ProjectFromFolder(args[0],
                    new List<ILibrary>
                    {
                        new IO(),
                        new Lp()
                    }));
            if (runtime.HasErrors)
                System.Console.WriteLine(runtime.ToString());
        }
    }
}