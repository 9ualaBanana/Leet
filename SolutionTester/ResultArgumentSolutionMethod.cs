using System.Reflection;

namespace CCHelper;

internal class ResultArgumentSolutionMethod : SolutionMethod
{
    internal override Type ResultType => _resultType ??= _method.GetParameters().Single(parameter => parameter.GetCustomAttribute(typeof(ResultAttribute)) is not null).ParameterType;

    internal ResultArgumentSolutionMethod(MethodInfo method, object solutionContainer) : base(method, solutionContainer) 
    {
    }

    protected override void ProcessResult(object? _)
    {
        var resultParameterPosition = _method.GetParameters().Single(parameter => parameter.GetCustomAttribute(typeof(ResultAttribute)) is not null).Position;
        Result = Arguments[resultParameterPosition];
    }
}