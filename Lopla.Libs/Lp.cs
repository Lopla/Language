namespace Lopla.Libs
{
    using System;
    using Language.Binary;
    using Language.Libraries;
    using Language.Processing;
    using String = Language.Binary.String;

    public class Lp : BaseLoplaLibrary
    {
        private string[] args;

        public Lp(params string []args)
        {
            this.args = args;
            Add("Len", Len, "array");
            Add("Floor", Floor, "number");
            Add("Args", Args);
        }

        private Result Args(Mnemonic expression, Runtime runtime)
        {
            var ll = new LoplaList();
            if (args != null)
            {
                foreach (var a in args)
                {
                    ll.Add(new Result(new String(a)));
                }
            }
            
            return new Result(ll);
        }

        private Result Floor(Mnemonic expression, Runtime runtime)
        {
            var n = GetArgument<Number>("number", runtime);
            return new Result(new Number(Math.Floor(n.Value)));
        }

        private Result Len(Mnemonic expression, Runtime runtime)
        {
            var a = runtime.GetVariable("array").Get(runtime);
            if (a is String arg2)
            {
                return new Result(new Number(arg2.Value.Length));
            }
            else if(a is LoplaList lp)
            {
                return new Result(new Number(lp.Length));
            }
            return new Result();
        }
    }
}