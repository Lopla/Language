using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lopla.Language.Binary;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;
using String = Lopla.Language.Binary.String;

namespace Lopla.Tests.Logic.Mocks
{
    public class Test : BaseLoplaLibrary
    {
        private readonly List<string> _console;

        public Test(List<string> console)
        {
            _console = console;

            Add("Write", Write, "text");
        }

        private Result Write(Mnemonic expression, Runtime runtime)
        {
            var arg = runtime.GetVariable("text");

            if (arg.Get(runtime) is String line)
            {
                Debug.WriteLine("TEST:WRITE: " + line.Value);
                Console.WriteLine("TEST:WRITE: " + line.Value);
                _console.Add(line.Value);
            }
            else if (arg.Get(runtime) is Number data)
            {
                Debug.WriteLine("TEST:WRITE: " + data.Value);
                Console.WriteLine("TEST:WRITE: " + data.Value);
                _console.Add(data.Value.ToString());
            }

            return new Result();
        }
    }
}