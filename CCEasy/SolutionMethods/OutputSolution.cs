using System.Reflection;

namespace CCEasy.SolutionMethods;

internal class OutputSolution<TResult> : SolutionMethod<TResult>
{
    protected override Type ResultType => _method.ReturnType;

    internal OutputSolution(MethodInfo method, object solutionContainer)
        : base(method, solutionContainer)
    {
    }

    protected override void Init()
    {
    }

    protected override object? RetrieveSolutionResult(object? methodInfoResult)
    {
        return methodInfoResult;
    }
}