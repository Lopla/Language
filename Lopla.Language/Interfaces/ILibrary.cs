using System.Collections.Generic;
using Lopla.Language.Binary;
using Lopla.Language.Environment;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;

namespace Lopla.Language.Interfaces
{
    public interface ILibrary
    {
        string Name { get; }
        IEnumerable<KeyValuePair<MethodPointer, Method>> Methods();
        Result Call(DoHandler action, Mnemonic context, IRuntime runtime);
    }
}