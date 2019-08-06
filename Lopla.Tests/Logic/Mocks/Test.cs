using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Lopla.Language.Binary;
using Lopla.Language.Interfaces;
using Lopla.Language.Libraries;
using Xunit.Abstractions;
using String = Lopla.Language.Binary.String;

namespace Lopla.Tests.Logic.Mocks
{
    public class Test : BaseLoplaLibrary
    {
        private readonly List<string> _console;
        private readonly ITestOutputHelper _logger;
        private string line = "";

        public Test(List<string> console, ITestOutputHelper logger)
        {
            _console = console;
            this._logger = logger;

            Add("Write", Write, "text");
            Add("ToString", ToString, "number");
        }

        private Result ToString(Mnemonic expression, IRuntime runtime)
        {
            var arg = runtime.GetVariable("number");
            
            if (arg.Get(runtime) is Number data)
            {
                var r = data.Value.ToString(CultureInfo.InvariantCulture);
                return new Result(new String(r));
            }

            return new Result();
        }

        private Result Write(Mnemonic expression, IRuntime runtime)
        {
            var arg = runtime.GetVariable("text");

            if (arg.Get(runtime) is String line)
            {
                Debug.WriteLine("TEST:WRITE: " + line.Value);
                Console.WriteLine("TEST:WRITE: " + line.Value);
                _logger.WriteLine("TEST:WRITE: " + line.Value);
                _console.Add(line.Value);
            }
            else if (arg.Get(runtime) is Number data)
            {
                var r = data.Value.ToString(CultureInfo.InvariantCulture);
                Debug.WriteLine("TEST:WRITE: " + r);
                Console.WriteLine("TEST:WRITE: " + r);
                _logger.WriteLine("TEST:WRITE: " + r);
                _console.Add(r);
            }

            return new Result();
        }
    }
}