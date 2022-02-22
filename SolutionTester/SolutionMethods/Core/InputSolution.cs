using System.Reflection;

namespace CCHelper;

/// <summary>
/// Labels a solution method that provides its result in one of the arguments.
/// </summary>
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
        Guard.Against.Null(Arguments, nameof(Arguments), "Arguments can't be null for InputSolution.");

        var resultParameterPosition = _method.GetParameters()
            .Single(parameter => parameter.IsDefined(typeof(ResultAttribute)))
            .Position;
        return Arguments[resultParameterPosition];
    }
}