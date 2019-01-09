namespace Lopla.Language.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Compiler;
    using Interfaces;

    public class Runner
    {
        private Runtime _runtime;

        public ParseCompileAndRunErrors Run(IProject project, Processors processors = null)
        {
            var compileResult = Compile(project);

            if (compileResult.Any(c => c.HasErrors))
            {
                var result = new ParseAndCompileResult();
                foreach (var error in compileResult.SelectMany(e => e.Errors)) result.Errors.Add(error);
                return result;
            }

            return this.Start(processors, project.Libs(), compileResult);
        }

        public ParseCompileAndRunErrors Start(
            Processors processors, 
            IEnumerable<ILibrary> libs,
            List<ParseAndCompileResult> compileResult)
        {
            var result = new ParseAndCompileResult();
            if (processors == null)
            {
                processors = new Processors();
            }

            _runtime = new Runtime(processors);

            //// link
            foreach (var library in libs) _runtime.Link(library);
            foreach (var binary in compileResult) 
            {
                _runtime.Run(binary.Compilation);
                if(_runtime.Errors.Count()> 0){
                    break;
                }
            }
            foreach (var runtimeError in _runtime.Errors) result.Errors.Add(runtimeError);

            return result;
        }

        public List<ParseAndCompileResult> Compile(IProject project)
        {
            //// parse and compile
            var parsingEngine = new Parser();

            var binaries = new List<ParseAndCompileResult>();

            foreach (var script in project.Scripts())
            {
                var fileResult = parsingEngine.ParseAndCompile(script);
                if (fileResult.Errors.Any())
                    fileResult.Compilation = null;
                binaries.Add(fileResult);
            }

            return binaries;
        }

        public void Stop()
        {
            _runtime?.Stop();
        }
    }
}