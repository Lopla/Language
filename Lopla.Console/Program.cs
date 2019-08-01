using System.Collections.Generic;
using Lopla.Language.Interfaces;
using Lopla.Libs;
using Lopla.Starting;

namespace Lopla.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new MainHandler(new List<ILibrary>
            {
                new IO(),
                new Lp()
            }).Main(args);
        }
    }
}