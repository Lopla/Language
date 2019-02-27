using Lopla.Language.Errors;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;

namespace Lopla.Language.Compiler
{
    using System.Linq;
    using Hime.Redist;

    internal static class NodesHelpers
    {
        public static ASTNode? GetChildAndAddError(this ASTNode node, int i, string tokenName, IMnemonicsCompiler runtime, bool optional = false)
        {
            ASTNode? n = node;
            return n.GetChildAndAddError(i, tokenName, runtime, optional);
        }

        public static ASTNode? GetChildAndAddError(this ASTNode? node, int i, string tokenName, IMnemonicsCompiler runtime,
            bool optional = false)
        {
            if (node != null)
            {
                if (node.Value.Children.Count >= i + 1)
                {
                    var element = node.Value.Children.ElementAt(i);
                    if (element.Symbol.Name != tokenName)
                    {
                        if (!optional)
                            runtime.AddError(new CompilationError($"Expected: {tokenName} but found: {element.Symbol.Name}."));
                    }
                    else
                    {
                        return element;
                    }
                }
                else if (!optional)
                {
                    runtime.AddError(new CompilationError($"Expected: {i} tokens but found: {node.Value.Children.Count} when looking for: {tokenName}."));
                }
            }
            else
            {
                if (!optional)
                    runtime.AddError(new CompilationError($"Parent node for: {tokenName} not found."));
            }

            return null;
        }
    }
}