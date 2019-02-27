using System.Collections.Generic;
using Lopla.Language.Binary;
using Lopla.Language.Environment;

namespace Lopla.Language.Interfaces
{
    public interface ILibrary
    {
        IEnumerable<KeyValuePair<MethodPointer, Method>> Methods();
        string Name { get; }
    }
}