using System.Collections.Generic;
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
            RunProject();
        }

        private static void RunProject()
        {
            //// project is poor 
            //// only few dll's
            var p = new Runner();
            var runtime = p.Run(
                new MemoryScripts("Test",
                    new List<ILibrary>
                    {
                        new IO(),
                        new Lp()
                    },
                    @"
libs = Lp.Functions()
k =0

while(k<Lp.Len(libs)) {
    d = libs[k]
    IO.WriteLine(d[0])
    k=k+1
}
IO.WriteLine(""test"")
"));
            if (runtime.HasErrors)
                System.Console.WriteLine(runtime.ToString());
        }
    }
}