using System;
using Lopla.Language.Binary;
using Lopla.Language.Compiler.Mnemonics;
using Lopla.Language.Environment;
using Lopla.Language.Processing;
using Xunit;
using String = Lopla.Language.Binary.String;

namespace Lopla.Tests
{
    public class GlobalScopeSpecs
    {
        [Fact]
        public void RemebersTheVaraibleValueAfterModificationInSubScope()
        {
            var sut = new GlobalScope(null, "a");
            sut.SetVariable("a", new String("bottom"), false);
            
            sut.StartScope();
            sut.SetVariable("a", new String("derived"), true);
            var innerAa = 
                sut
                .GetVariable("a", new Runtime(new Processors())).Get(new Runtime(new Processors())) as String;
            Assert.Equal("derived", innerAa.Value);
            sut.EndScope();

            var a = sut.GetVariable("a", new Runtime(new Processors())).Get(new Runtime(new Processors())) as String;

            Assert.Equal("bottom", a.Value);
        }

        [Fact]
        public void DoesNotTModifyVariableInUpperFromDerivedScope()
        {
            var sut = new GlobalScope(null, "root");
            
            sut.SetVariable("a", new String("bottom"), false);

            var derived = sut.DeriveFunctionScope();
            derived.SetVariable("a", new String("derived"), true);
            var innerAa = derived.GetVariable("a", new Runtime(new Processors())).Get(new Runtime(new Processors())) as String;
            Assert.Equal("derived", innerAa.Value);
            

            var a = sut.GetVariable("a", new Runtime(new Processors())).Get(new Runtime(new Processors())) as String;

            Assert.Equal("bottom", a.Value);
        }


    }
}