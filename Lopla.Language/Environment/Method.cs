using System.Collections.Generic;
using Lopla.Language.Binary;

namespace Lopla.Language.Environment
{
    public class Method : IArgument
    {
        public Method()
        {
            this.ArgumentList=new List<string>();
        }

        /// <summary>
        ///     Method code
        /// </summary>
        public List<Mnemonic> Code { get; set; }

        public List<string> ArgumentList { get; set; }
        
        public string GlobalScopeName { get; set; }
    }
}