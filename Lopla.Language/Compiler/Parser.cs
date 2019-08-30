namespace Lopla.Language.Compiler
{
    using System;
    using System.Linq;
    using Grammar;
    using Handler;
    using Hime.Redist;
    using ParseError = Errors.ParseError;

    internal class Parser
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

                    var line = parseResult.Input.GetLineContent(e.Position.Line).Replace('\t', ' ');
                    var error = $"{script.Name}:{e.Position.Line}[{e.Position.Column}] {e.Message}.";
                    return
                        new ParseError(
                            //line +
                            // new string(' ', e.Position.Column) + "^" + Environment.NewLine +
                            // new string(' ', e.Position.Column) + "| " + 
                            error +
                            (utname != null ? $" (Unexpected token was: {utname})" : ""));
                }).ToList().ForEach(e => { processingResult.Errors.Add(e); });

                //// no errors, then try to compile
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

        private void Show(ASTNode resultRoot, string sep = "")
        {
            Console.WriteLine($"{sep}{resultRoot.Symbol}");
            foreach (var resultRootChild in resultRoot.Children)
                Show(resultRootChild, sep + "\t");
        }
    }
}