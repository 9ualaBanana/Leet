using System.Reflection;

namespace CCHelper;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class SolutionAttribute : Attribute
{
}

internal class OutputSolution : SolutionMethod
{
    internal override Type ResultType => _resultType ??= _method.ReturnType;

    internal OutputSolution(MethodInfo method, object solutionContainer) : base(method, solutionContainer, SolutionMethodValidator.IsValidOutputSolution)
    {
    }

    protected override void ProcessResult(object? rawResult)
    {
        Result = rawResult;
    }
}