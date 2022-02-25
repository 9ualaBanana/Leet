using System.Reflection;

namespace CCHelper;

/// <summary>
/// Labels a solution method that provides the result in its return value.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class SolutionAttribute : Attribute
{
}

internal class OutputSolution<TResult> : SolutionMethod<TResult>
{
    internal OutputSolution(MethodInfo method, object solutionContainer) 
        : base(method, solutionContainer, SolutionMethodValidator.IsValidOutputSolution)
    {
    }

    protected override object? RetrieveSolutionMethodSpecificResult(object? methodInfoResult)
    {
        return methodInfoResult;
    }
}