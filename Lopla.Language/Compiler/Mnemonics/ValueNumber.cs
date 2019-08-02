using System;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    using System.Globalization;

    public class ValueNumber : Mnemonic
    {
        public Number Value;

        public ValueNumber(ASTNode? node) : base(node)
        {
            Value = new Number(Convert.ToDecimal(node.Value.Value, CultureInfo.InvariantCulture));
        }

        public ValueNumber(Number val) : base(null)
        {
            Value = val;
        }

        public override Result Execute(IRuntime runtime)
        {
            return new Result(Value);
        }
    }
}