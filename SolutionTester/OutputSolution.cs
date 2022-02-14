using System.Reflection;

namespace CCHelper;

internal class OutputSolution : SolutionMethod
{
    internal override Type ResultType => _resultType ??= _method.ReturnType;

    internal OutputSolution(MethodInfo method, object solutionContainer) : base(method, solutionContainer)
    {
    }
    protected override void EnsureSolutionMethodIsValid(MethodInfo method)
    {
        if (!method.IsValidOutputSolution()) throw new InvalidOperationException("Wrong method was identified as OutputSolution");
    }

    protected override void ProcessResult(object? rawResult)
    {
        Result = rawResult;
    }
}