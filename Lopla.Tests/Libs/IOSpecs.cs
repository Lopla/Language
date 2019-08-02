namespace Lopla.Tests.Libs
{
    using Language.Binary;
    using Language.Compiler.Mnemonics;
    using Language.Processing;
    using Lopla.Libs;
    using Xunit;

    public class IOSpecs
    {
        public void Aaaaa()
        {
            var sut = new IO();
            sut.Write(new ValueNumber(new Number((decimal) 3.145)), new Runtime(new Processors()));
        }
    }
}