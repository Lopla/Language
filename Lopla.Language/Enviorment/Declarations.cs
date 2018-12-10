namespace Lopla.Language.Enviorment
{
    using System.Collections.Generic;
    using Binary;
    using Errors;
    using Processing;

    public class Declarations
    {
        private readonly Dictionary<string, Method> _procedures = new Dictionary<string, Method>();

        private static Dictionary<string, Result> ExtractArguments(List<Result> functionParamters, Method m)
        {
            var args = new Dictionary<string, Result>();

            //// copy arguments into memory and set them as values
            if (m.ArgumentList != null && functionParamters != null)
                for (var k = 0; k < m.ArgumentList.Count; k++)
                    args.Add(m.ArgumentList[k], functionParamters[k]);

            return args;
        }

        public void Register(MethodPointer name, Method method, Runtime runtime, string processingStackName)
        {
            var methodName = $"{name.NameSpace}.{name.Name}";
            if (!_procedures.ContainsKey(methodName))
            {
                method.GlobalScopeName = processingStackName;
                _procedures.Add(methodName, method);
            }
            else
            {
                runtime.AddError(new LinkingError($"Failed to reregister method {methodName} (already declared)"));
            }
        }

        public Dictionary<string, Result> GetArguments(MethodPointer p, List<Result> functionParamters, Runtime runtime)
        {
            var mName = p.NameSpace + "." + p.Name;
            if (_procedures.ContainsKey(mName))
            {
                var functionRefernece = _procedures[mName];

                //// just check if user providede all paramters
                if (functionRefernece.ArgumentList?.Count != functionParamters?.Count)
                {
                    runtime.AddError(new RuntimeError(
                        $"Invalid number of paramters passed to function {p.NameSpace}.{p.Name}. Expected {functionRefernece.ArgumentList?.Count} provided {functionParamters?.Count}",
                        null));
                }
                else
                {
                    var args = ExtractArguments(functionParamters, functionRefernece);
                    return args;
                }
            }
            else
            {
                runtime.AddError(new RuntimeError($"Method not found {p.NameSpace}.{p.Name}", null));
            }

            return null;
        }

        public string GetScope(MethodPointer p, Runtime runtime)
        {
            var mName = p.NameSpace + "." + p.Name;
            if (_procedures.ContainsKey(mName))
                return _procedures[mName].GlobalScopeName;
            runtime.AddError(new RuntimeError($"Method not found {p.NameSpace}.{p.Name}", null));

            return null;
        }

        public List<Mnemonic> GetCode(MethodPointer p, Runtime runtime)
        {
            var mName = p.NameSpace + "." + p.Name;
            if (_procedures.ContainsKey(mName))
                return _procedures[mName].Code;
            runtime.AddError(new RuntimeError($"Method not found {p.NameSpace}.{p.Name}", null));

            return null;
        }
    }
}