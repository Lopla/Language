namespace Lopla.Language.Binary
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    ///     In memory compiled script with list of expressions
    /// </summary>
    [DebuggerDisplay("{Code}")]
    public class Compilation
    {
        public Compilation(string name)
        {
            Mnemonics = new List<Mnemonic>();
            Name = name;
        }

        public string Name { get; protected set; }

        public List<Mnemonic> Mnemonics { get; set; }

        public string Code
        {
            get { return string.Join(Environment.NewLine, Mnemonics.Select(m => m.ToString())); }
        }
        
        public void Add(Mnemonic expression)
        {
            Mnemonics.Add(expression);
        }
    }
}