using System.Collections.Generic;
using Lopla.Language.Compiler;

namespace Lopla.Language.Interfaces
{
    public interface IProject
    {
        string Name { get; }
        IEnumerable<Script> Scripts();
        IEnumerable<ILibrary> Libs();
        IEnumerable<IMedia> Media();
    }
}