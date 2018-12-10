namespace Lopla.Language.Binary
{
    using System.Collections.Generic;

    public class ClassAssigments : IArgument
    {
        public readonly List<Mnemonic> Mnemonics = new List<Mnemonic>();

        public void Add(Mnemonic assigment)
        {
            Mnemonics.Add(assigment);
        }
    }
}