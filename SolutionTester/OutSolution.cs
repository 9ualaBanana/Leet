using System.Reflection;

namespace CCHelper;

internal class OutSolution : SolutionMethod
{
    internal override Type ResultType => _resultType ??= _method.ReturnType;

    internal OutSolution(MethodInfo method, object solutionContainer) : base(method, solutionContainer)
    {
    }

    protected override void ProcessResult(object? rawResult)
    {
        Result = rawResult;
    }
}