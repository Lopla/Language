using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lopla.Language.Binary;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;
using Xunit.Abstractions;
using String = Lopla.Language.Binary.String;

namespace Lopla.Tests.Logic.Mocks
{
    using System.Globalization;

    public class Test : BaseLoplaLibrary
    {
        private readonly List<string> _console;
        private ITestOutputHelper logger;

        public Test(List<string> console, ITestOutputHelper logger)
        {
            _console = console;
            this.logger = logger;

            Add("Write", Write, "text");
        }

        private Result Write(Mnemonic expression, Runtime runtime)
        {
            var arg = runtime.GetVariable("text");

            if (arg.Get(runtime) is String line)
            {
                Debug.WriteLine("TEST:WRITE: " + line.Value);
                Console.WriteLine("TEST:WRITE: " + line.Value);
                logger.WriteLine("TEST:WRITE: " + line.Value);
                _console.Add(line.Value);
            }
            else if (arg.Get(runtime) is Number data)
            {
                string r = data.Value.ToString(CultureInfo.InvariantCulture);
                Debug.WriteLine("TEST:WRITE: " + r);
                Console.WriteLine("TEST:WRITE: " + r);
                logger.WriteLine("TEST:WRITE: " + r);
                _console.Add(r);
            }

            return new Result();
        }
    }
}