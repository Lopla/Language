using System.Collections.Generic;
using System.Linq;
using Lopla.Language.Binary;
using Lopla.Language.Errors;

namespace Lopla.Language.Compiler
{
    public class ParseCompileAndRunErrors
    {
        public readonly List<Error> Errors = new List<Error>();
        public bool HasErrors => Errors.Any();

        public override string ToString()
        {
            List<string> errors = new List<string>();
            bool runtime = false;
            foreach (var error in Errors)
            {
                if (error is RuntimeError)
                {
                    if(runtime==false)
                        errors.Add(error.Text);
                    runtime = true;
                }
                else
                {
                    errors.Add(error.Text);
                }
            }

            return string.Join(System.Environment.NewLine, errors);
        }
    }

    public class ParseAndCompileResult : ParseCompileAndRunErrors
    {
        public Compilation Compilation { get; set; }
    }
}