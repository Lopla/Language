using System;
using System.Collections.Generic;
using Lopla.Language.Compiler;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;
using Lopla.Language.Providers;
using Lopla.Tests.Logic.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace Lopla.Tests.Logic
{
    public abstract class ParserTestBase
    {
        private ITestOutputHelper logger;

        protected ParserTestBase(ITestOutputHelper logger)
        {
            this.logger = logger;
        }


        protected void EvaluateCode(string script, params string[] args)
        {
            var consoleText = new List<string>();

            var testScripts = new MemoryScripts(
                "test",
                new List<ILibrary>
            {
                new Test(consoleText, logger)
            })
            {
                Files = new List<Script>
                {
                    new Script
                    {
                        Content = script,
                        Name = "TestScript"
                    }
                }
            };

            var r = new Runner();
            var result = r.Run(testScripts);

            Assert.False(result.HasErrors, string.Join(Environment.NewLine, result.ToString()));

            if (!result.HasErrors)
            {
                Assert.Equal(args.Length, consoleText.Count);

                if (args.Length == consoleText.Count)
                    for (var x = 0; x < args.Length; x++)
                        Assert.Equal(args[x], consoleText[x]);
            }
        }
    }
}