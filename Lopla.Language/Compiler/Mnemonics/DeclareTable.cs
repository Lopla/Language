using System.Collections.Generic;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Enviorment;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler.Mnemonics
{
    public class DeclareTable : Mnemonic
    {
        public DeclareTable(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            this.Values=new List<Mnemonic>();
            
            var kids = node.GetChildAndAddError(0, "declare_table_values", runtime, true);
            if (kids.HasValue && kids.Value.Children.Count > 0)
            {
                foreach (var valueChild in kids.Value.Children)
                {
                    this.Values.Add(runtime.Get(valueChild));
                }
            }
        }

        public List<Mnemonic> Values { get; set; }

        public override Result Execute(Runtime runtime)
        {
            var result = new LoplaList();
            int k = 0;
            foreach (var baseMnemonic in this.Values)
            {
                result.Set(k, runtime.EvaluateCodeBlock(baseMnemonic));
                k++;
            }

            return new Result(result);
        }
    }
}