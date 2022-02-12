using System.Reflection;

namespace CCHelper;

internal class ReturnValueSolutionMethod : SolutionMethod
{
    internal override Type ResultType => _resultType ??= _method.ReturnType;

    internal ReturnValueSolutionMethod(MethodInfo method, object solutionContainer) : base(method, solutionContainer)
    {
    }

    protected override void ProcessResult(object? rawResult)
    {
        Result = rawResult;
    }
}