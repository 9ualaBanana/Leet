using System.Reflection;

namespace CCHelper;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public class ResultAttribute : Attribute
{
}

// Arguments are never null for this type of SolutionMethod as [ResultAttribute] can only be applied to parameters
// and this class is instantiated only when this attribute is present (it's checked by SolutionMethodValidator).
internal class InputSolution : SolutionMethod
{
    internal override Type ResultType => 
        _resultType ??= _method.GetParameters()
                               .Single(parameter => parameter.GetCustomAttribute(typeof(ResultAttribute)) is not null)
                               .ParameterType;

    internal InputSolution(MethodInfo method, object solutionContainer) : base(method, solutionContainer) 
    {
    }
    protected override void EnsureSolutionMethodIsValid(MethodInfo method)
    {
        if (!method.IsValidInputSolution()) throw new InvalidOperationException("Wrong method was identified as InputSolution");
    }

    protected override void ProcessResult(object? _)
    {
        var resultParameterPosition = _method.GetParameters().Single(parameter => parameter.GetCustomAttribute(typeof(ResultAttribute)) is not null).Position;

        Result = Arguments![resultParameterPosition];
    }
}