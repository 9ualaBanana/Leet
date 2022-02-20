using System.Reflection;

namespace CCHelper;
// InputSolution and OutputSolution contain their corresponding attribute definition, solution method validation logic and result setting logic.
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

    protected override object? RetrieveSolutionMethodSpecificResult(object? rawResult)
    {
        return rawResult;
    }
}