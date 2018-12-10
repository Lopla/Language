namespace Lopla.Libs
{
    using System;
    using Language.Binary;
    using Language.Errors;
    using Language.Libraries;
    using Language.Processing;
    using String = Language.Binary.String;

    // ReSharper disable once InconsistentNaming
    public class IO : BaseLoplaLibrary
    {
        private readonly string _prefix = "";

        public IO()
        {
            Add("Write", Write, "text");
            Add("WriteLine", WriteLine, "text");
            Add("Read", Read);
        }

        private Result WriteLine(Mnemonic expression, Runtime runtime)
        {
            var arg = runtime.GetVariable("text");
            if (arg.Get(runtime) is String line) Console.WriteLine(_prefix + line.Value);
            else if (arg.Get(runtime) is Number i)
                Console.WriteLine(_prefix + i.Value);
            else
                runtime.AddError(new RuntimeError($"Type not supported {arg.Get(runtime).GetType().Name}"));
            return new Result();
        }

        private Result Write(Mnemonic expression, Runtime runtime)
        {
            var arg = runtime.GetVariable("text");
            if (arg.Get(runtime) is String line) Console.Write(_prefix + line.Value);
            else if (arg.Get(runtime) is Number i)
                Console.Write(_prefix + i.Value);
            else
                runtime.AddError(new RuntimeError($"Type not supported {arg.Get(runtime).GetType().Name}"));
            return new Result();
        }

        private Result Read(Mnemonic expression, Runtime runtime)
        {
            Console.Write(_prefix);
            var data = Console.ReadLine();

            return new Result(new String
            {
                Value = data
            });
        }
    }
}