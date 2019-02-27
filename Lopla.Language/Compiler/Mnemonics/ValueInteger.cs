using System;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class ValueInteger : Mnemonic
    {
        public Number Value;

        public ValueInteger(ASTNode? node) : base(node)
        {
            Value = new Number(Convert.ToInt32(node.Value.Value));
        }

        public ValueInteger(Number val) : base(null)
        {
            Value = val;
        }

        public override Result Execute(Runtime runtime)
        {
            return new Result(Value);
        }
    }
}