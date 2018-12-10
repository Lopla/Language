namespace Lopla.Language.Binary
{
    using System.Collections.Generic;

    public class Arguments
    {
        public Arguments()
        {
            Args = new List<IArgument>();
        }

        public Arguments(params IArgument[] value)
            : this()
        {
            if (value != null) Args.AddRange(value);
        }

        public List<IArgument> Args { get; set; }
    }
}