using System.Reflection;

namespace CCHelper;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public class ResultAttribute : Attribute
{
}


internal class InputSolution<TResult> : SolutionMethod<TResult>
{
    internal InputSolution(MethodInfo method, object solutionContainer) 
        : base(method, solutionContainer, SolutionMethodValidator.IsValidInputSolution) 
    {
    }

    protected override object? RetrieveSolutionMethodSpecificResult(object? _)
    {
        var resultParameterPosition = _method.GetParameters()
            .Single(parameter => parameter.IsDefined(typeof(ResultAttribute))).Position;
        // Arguments are never null for InputSolution.
        return Arguments![resultParameterPosition];
    }
}