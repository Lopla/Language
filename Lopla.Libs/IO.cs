using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;
using String = Lopla.Language.Binary.String;

namespace Lopla.Libs
{
    public class IO : BaseLoplaLibrary
    {
        private readonly string _prefix ="";
        public IO()
        {
            Add("Write", Write, "text");
            Add("WriteLine", WriteLine, "text");
            Add("Read", Read);
        }

        private Result WriteLine(Mnemonic expression, Runtime runtime)
        {
            var arg = runtime.GetVariable("text");
            if (arg.Get(runtime) is String line) System.Console.WriteLine(_prefix + line.Value);
            else if (arg.Get(runtime) is Number i)
                System.Console.WriteLine(_prefix + i.Value );
            else
                runtime.AddError(new RuntimeError($"Type not supported {arg.Get(runtime).GetType().Name}"));
            return new Result();
        }

        private Result Write(Mnemonic expression, Runtime runtime)
        {
            var arg = runtime.GetVariable("text");
            if (arg.Get(runtime) is String line) System.Console.Write(_prefix + line.Value);
            else if (arg.Get(runtime) is Number i)
                System.Console.Write(_prefix + i.Value);
            else
                runtime.AddError(new RuntimeError($"Type not supported {arg.Get(runtime).GetType().Name}"));
            return new Result();
        }

        private Result Read(Mnemonic expression, Runtime runtime)
        {
            System.Console.Write(_prefix);
            var data = System.Console.ReadLine();

            return new Result(new String
            {
                Value = data
            });
        }
    }
}