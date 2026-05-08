namespace Lopla.Language.Compiler
{
    using System;
    using System.Linq;
    using Grammar;
    using Handler;
    using Hime.Redist;
    using ParseError = Errors.ParseError;

    public class LanguageService
    {
        public ParseAndCompileResult ParseAndCompile(Script script)
        {
            var processingResult = new ParseAndCompileResult();
            if (!string.IsNullOrWhiteSpace(script.Content))
            {
                var lexer = new LoplaLexer(script.Content);
                var parser = new LoplaParser(lexer, new LoplaActions());
                var parseResult = parser.Parse();

                parseResult.Errors.Select(e =>
                {
                    string utname = null;
                    if (e is UnexpectedTokenError ete)
                        utname = ete.UnexpectedToken.Symbol.Name;

                    var error = $"{script.Name}:{e.Position.Line}[{e.Position.Column}] {e.Message}.";
                    return new ParseError(
                        error +
                        (utname != null ? $" (Unexpected token was: {utname})" : ""));
                }).ToList().ForEach(e => processingResult.Errors.Add(e));

                processingResult.Root = ConvertTree(parseResult.Root);

                if (!processingResult.HasErrors)
                {
                    var compilate = new Compiler().Compile(parseResult.Root, script.Name);

                    if (compilate.Errors.Any())
                        processingResult.Errors.AddRange(compilate.Errors);
                    else
                        processingResult.Compilation = compilate.Compilate;
                }
            }

            return processingResult;
        }

        private static ParseTreeNode ConvertTree(ASTNode node)
        {
            var item = new ParseTreeNode
            {
                Symbol = node.Symbol.Name,
                Value = node.Value?.Trim() ?? string.Empty
            };

            foreach (var child in node.Children)
            {
                var childNode = ConvertTree(child);
                if (childNode != null)
                    item.Children.Add(childNode);
            }

            return item;
        }
    }
}
