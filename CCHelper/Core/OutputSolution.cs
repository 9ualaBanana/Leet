using System.Reflection;

namespace CCHelper.Core;

internal class OutputSolution<TResult> : SolutionMethod<TResult>
{
    protected override Type ResultType => _method.ReturnType;

    internal OutputSolution(MethodInfo method, object solutionContainer) 
        : base(method, solutionContainer)
    {
        EnsureResultsTypesCompatibility();
    }

    protected override object? RetrieveSolutionMethodSpecificResult(object? methodInfoResult)
    {
        return methodInfoResult;
    }
}