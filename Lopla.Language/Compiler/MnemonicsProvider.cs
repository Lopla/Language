using System.Collections.Generic;
using Hime.Redist;
using Lopla.Language.Binary;
using Lopla.Language.Compiler.Mnemonics;
using Lopla.Language.Errors;
using Lopla.Language.Interfaces;

namespace Lopla.Language.Compiler
{
    public class MnemonicsProvider : IMnemonicsCompiler
    {
        public MnemonicsProvider()
        {
            Errors = new List<CompilationError>();
        }

        public List<CompilationError> Errors { get; }

        public Mnemonic Get(ASTNode? node)
        {
            Mnemonic result = null;
            var symbol = node.Value.Symbol.Name;

            switch (symbol)
            {
                case "assigment":
                    result = new Assigment(node, this);
                    break;
                case "expression":
                    result = new Expression(node, this);
                    break;
                case "expression_arg":
                case "expression_sum":
                case "expression_mult":
                case "expression_cmp":
                case "expression_bracket":
                case "expression_bool":
                    result = new ExpressionCommon(node, this);
                    break;
                case "expression_prefix":
                    result = new ExpressionPrefix(node, this);
                    break;
                case "NUMBER":
                    result = new ValueInteger(node);
                    break;
                case "STRING":
                    result = new ValueString(node);
                    break;
                case "method_call":
                    result = new MethodCall(node, this);
                    break;
                case "emptyLine":
                    result = new Nop(node);
                    break;
                case "variable_name":
                    result = new VariableName(node, this);
                    break;
                case "if":
                    result = new If(node, this);
                    break;
                case "block_of_lopla":
                    result = new Block(node, this);
                    break;
                case "method":
                    result = new MethodDeclaration(node, this);
                    break;
                case "declare_table":
                    result = new DeclareTable(node, this);
                    break;
                case "var_value_table":
                    result = new ValueTable(node, this);
                    break;
                case "while":
                    result = new While(node, this);
                    break;
                case "return":
                    result = new Return(node, this);
                    break;
                default:
                    AddError(
                        new CompilationError($"{symbol} not handled by compiler. line: {node?.Position.Line}"));
                    break;
            }

            return result;
        }

        public void AddError(CompilationError e)
        {
            Errors.Add(e);
        }
    }
}