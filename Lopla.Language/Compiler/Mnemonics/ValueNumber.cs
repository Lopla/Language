using System;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class ValueNumber : Mnemonic
    {
        public Number Value;

        public ValueNumber(ASTNode? node) : base(node)
        {
            Value = new Number(Convert.ToDecimal(node.Value.Value));
        }

        public ValueNumber(Number val) : base(null)
        {
            Value = val;
        }

        public override Result Execute(Runtime runtime)
        {
            return new Result(Value);
        }
    }
}