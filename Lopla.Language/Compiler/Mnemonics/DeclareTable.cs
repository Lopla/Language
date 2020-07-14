namespace Lopla.Language.Compiler.Mnemonics
{
    using System.Collections.Generic;
    using Binary;
    using Hime.Redist;
    using Interfaces;
    using Processing;

    public class DeclareTable : Mnemonic
    {
        public DeclareTable(ASTNode? node, IMnemonicsCompiler runtime) : base(node)
        {
            Values = new List<Mnemonic>();

            var kids = node.GetChildAndAddError(0, "declare_table_values", runtime, true);
            if (kids.HasValue && kids.Value.Children.Count > 0)
                foreach (var valueChild in kids.Value.Children)
                    Values.Add(runtime.Get(valueChild));
        }

        /// <summary>
        ///     Creates empty array
        /// </summary>
        /// <param name="node"></param>
        public DeclareTable(ASTNode? node) : base(node)
        {
            Values = new List<Mnemonic>();
        }

        public List<Mnemonic> Values { get; set; }

        public override Result Execute(IRuntime runtime)
        {
            var result = new LoplaList();
            var k = 0;
            foreach (var baseMnemonic in Values)
            {
                var resultData = runtime.EvaluateCodeBlock(baseMnemonic).Get(runtime);
                result.Set(k, resultData);
                k++;
            }

            return new Result(result);
        }
    }
}