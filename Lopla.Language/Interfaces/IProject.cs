namespace Lopla.Language.Interfaces
{
    using System.Collections.Generic;
    using Compiler;

    public interface IProject
    {
        string Name { get; }
        IEnumerable<Script> Scripts();
        IEnumerable<ILibrary> Libs();
        IEnumerable<IMedia> Media();
    }
}