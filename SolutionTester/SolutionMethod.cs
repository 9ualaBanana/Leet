using System.Reflection;

namespace CCHelper;

internal abstract class SolutionMethod
{
    protected MethodInfo _method;
    readonly object _solutionContainer;
    protected Type? _resultType;

    internal object?[]? Arguments { get; set; }
    protected object? Result { get; set; }
    internal abstract Type ResultType { get; }

    internal SolutionMethod(MethodInfo method, object solutionContainer)
    {
        _method = method;
        _solutionContainer = solutionContainer;
    }
    
    internal object? Invoke()
    {
        var rawResult = _method.Invoke(_solutionContainer, Arguments);
        ProcessResult(rawResult);
        return Result;
    }
    protected abstract void ProcessResult(object? rawResult);
}
