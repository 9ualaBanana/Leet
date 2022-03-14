using System.Reflection;

namespace CCEasy.Core;

internal class InputSolution<TResult> : SolutionMethod<TResult>
{
    ParameterInfo _resultParameter = null!;
    protected override Type ResultType => _resultParameter.ParameterType;

    internal InputSolution(MethodInfo method, object solutionContainer) 
        : base(method, solutionContainer) 
    {
    }

    protected override void Init()
    {
        _resultParameter = _method.GetParameters()
            .Single(parameter => parameter.IsDefined(typeof(ResultAttribute)));
    }

    protected override object? RetrieveSolutionResult(object? _)
    {
        return Arguments![_resultParameter.Position];
    }
}