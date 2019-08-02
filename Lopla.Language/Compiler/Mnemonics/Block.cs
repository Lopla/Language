using System.Collections.Generic;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class Block : Mnemonic
    {
        public Block(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            foreach (var valueChild in node.Value.Children)
            {
                var mnemonic = runtime.Get(valueChild);
                Lines.Add(mnemonic);
            }
        }

        public List<Mnemonic> Lines { get; } = new List<Mnemonic>();

        public override Result Execute(IRuntime runtime)
        {
            return runtime.EvaluateBlock(Lines);
        }

        public override string ToString()
        {
            return "Block {}";
        }
    }
}