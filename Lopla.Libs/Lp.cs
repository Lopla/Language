using System;
using Lopla.Language.Binary;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;
using String = Lopla.Language.Binary.String;

namespace Lopla.Libs
{
    public class Lp : BaseLoplaLibrary
    {
        private readonly string[] args;

        public Lp(params string[] args)
        {
            this.args = args;
            //// math
            Add("Len", Len, "array");
            Add("Floor", Floor, "number");
            Add("Ticks", Ticks);

            //// types
            Add("VarType", VarType, "variable");

            //// os
            Add("Args", Args);

            //// diagnostics / debug
            Add("Functions", Functions);
            Add("FunctionInfo", FunctionInfo, "functionName");
        }

        private Result VarType(Mnemonic expression, Runtime runtime)
        {
            var variableResult = runtime.GetVariable("variable");

            if (variableResult != null)
            {
                var data = variableResult.Get(runtime);

                return new Result(new String(data.GetType().Name));
            }
            else
            {
                return new Result();
            }
        }


        private Result FunctionInfo(Mnemonic expression, Runtime runtime)
        {
            var functionInformation = new LoplaList();
            var functionName = GetArgument<String>("functionName", runtime);
            functionInformation.Add(new Result(new String(functionName.Value)));

            return new Result(functionInformation);
        }

        /// <summary>
        ///     A single tick represents one hundred nanoseconds or one ten-millionth of a second.
        ///     There are 10,000 ticks in a millisecond, or 10 million ticks in a second.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="runtime"></param>
        /// <returns></returns>
        private Result Ticks(Mnemonic expression, Runtime runtime)
        {
            var now = DateTime.Now.Ticks;
            return new Result(new Number(now));
        }

        private Result Functions(Mnemonic expression, Runtime runtime)
        {
            var listOfLibs = new LoplaList();
            foreach (var registeredMethod in runtime.GetRegisteredMethods())
            {
                var methodLine = new LoplaList();
                methodLine.Add(new Result(new String(registeredMethod.Key)));
                foreach (var argument in registeredMethod.Value) methodLine.Add(new Result(new String(argument)));
                listOfLibs.Add(new Result(methodLine));
            }

            return new Result(listOfLibs);
        }

        private Result Args(Mnemonic expression, Runtime runtime)
        {
            var ll = new LoplaList();
            if (args != null)
                foreach (var a in args)
                    ll.Add(new Result(new String(a)));

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
                return new Result(new Number(arg2.Value.Length));
            if (a is LoplaList lp) return new Result(new Number(lp.Length));
            return new Result();
        }
    }
}