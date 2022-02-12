using System.Reflection;

namespace CCHelper;

internal abstract class SolutionMethod
{
    protected MethodInfo _method;
    readonly object _solutionContainer;
    object? _result;
    protected Type? _resultType;

    internal object?[]? Arguments { get; set; }
    protected object Result 
    {
        get => _result ?? throw new InvalidOperationException("Result can be accessed only after the method has been invoked.");
        set => _result = value;
    }
    internal abstract Type ResultType { get; }

    internal SolutionMethod(MethodInfo method, object solutionContainer)
    {
        _method = method;
        _solutionContainer = solutionContainer;
    }

    
    internal object Invoke()
    {
        var rawResult = _method.Invoke(_solutionContainer, Arguments);
        ProcessResult(rawResult);
        return Result;
    }
    protected abstract void ProcessResult(object? rawResult);

    internal static bool IsMalformed(MethodInfo method)
    {
        bool hasSolutionAttribute = method.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(SolutionAttribute));
        var resultAttributes = method.GetParameters().Where(parameter => parameter.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(ResultAttribute)));
        return !hasSolutionAttribute && !resultAttributes.Any()
            || hasSolutionAttribute && resultAttributes.Any()
            || hasSolutionAttribute && method.ReturnType == typeof(void)
            || resultAttributes.Count() > 1
            || resultAttributes.Any() && method.ReturnType != typeof(void);
    }
}
