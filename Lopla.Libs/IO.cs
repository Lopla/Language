using System.IO;
using Lopla.Language.Interfaces;

namespace Lopla.Libs
{
    using System;
    using System.Globalization;
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
            Add("LoadBinaryFile", LoadBinaryFile, "fileName");
        }

        private Result LoadBinaryFile(Mnemonic expression, IRuntime runtime)
        {
            var fileName = runtime.GetVariable("fileName")?.Get(runtime) as String;

            if (File.Exists(fileName.Value))
            {
                using (var stream = new BinaryReader(new FileStream(fileName.Value, FileMode.Open, FileAccess.Read)))
                {
                    
                    var data = stream.ReadBytes((int)stream.BaseStream.Length);

                    var base64Binary = Convert.ToBase64String(data);

                    //LoplaList loplaList = new LoplaList();
                    //foreach(var d in data)
                    //{
                    //    loplaList.Add(new Number(d));
                    //}

                    //return new Result(loplaList);
                    return new Result(new String(base64Binary));
                }
            }

            return new Result();
        }

        public Result WriteLine(Mnemonic expression, IRuntime runtime)
        {
            var arg = runtime.GetVariable("text");
            if (arg.Get(runtime) is String line) Console.WriteLine(_prefix + line.Value);
            else if (arg.Get(runtime) is Number i)
                Console.WriteLine(_prefix + i.Value.ToString(CultureInfo.InvariantCulture));
            else
                runtime.AddError(new RuntimeError($"Type not supported {arg.Get(runtime).GetType().Name}"));
            return new Result();
        }

        public Result Write(Mnemonic expression, IRuntime runtime)
        {
            var arg = runtime.GetVariable("text");
            if (arg.Get(runtime) is String line) Console.Write(_prefix + line.Value);
            else if (arg.Get(runtime) is Number i)
                Console.Write(_prefix + i.Value);
            else
                runtime.AddError(new RuntimeError($"Type not supported {arg.Get(runtime).GetType().Name}"));
            return new Result();
        }

        private Result Read(Mnemonic expression, IRuntime runtime)
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