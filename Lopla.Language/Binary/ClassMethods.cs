namespace Lopla.Language.Binary
{
    using System.Collections.Generic;

    public class ClassMethods : IArgument
    {
        public readonly List<Mnemonic> Mnemonics = new List<Mnemonic>();

        public void Add(Mnemonic get)
        {
            Mnemonics.Add(get);
        }
    }
}