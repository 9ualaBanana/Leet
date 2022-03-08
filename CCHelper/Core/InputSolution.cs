using System.Reflection;

namespace CCHelper.Core;

internal class InputSolution<TResult> : SolutionMethod<TResult>
{
    readonly ParameterInfo _resultParameter;
    protected override Type ResultType => _resultParameter.ParameterType;

    internal InputSolution(MethodInfo method, object solutionContainer) 
        : base(method, solutionContainer) 
    {
        _resultParameter = _method.GetParameters()
            .Single(parameter => parameter.IsDefined(typeof(ResultAttribute)));
        EnsureResultsTypesCompatibility();
    }

    protected override object? RetrieveSolutionMethodSpecificResult(object? _)
    {
        return Arguments![_resultParameter.Position];
    }
}