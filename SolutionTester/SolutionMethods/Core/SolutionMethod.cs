using System.Reflection;

namespace CCHelper;

internal abstract class SolutionMethod
{
    readonly object _solutionContainer;
    protected MethodInfo _method;
    Predicate<MethodInfo>? _isValid;
    object?[]? _arguments;
    protected Type? _resultType;

    internal object?[]? Arguments 
    { 
        get => _arguments;
        set
        {
            if (value is not null
                && !_method.GetParameters()
                          .Any(parameter => parameter.ParameterType.IsAssignableFrom(
                              value[parameter.Position]?.GetType())
                          ))
            {
                throw new ArgumentException("Types of provided arguments do not match the required parameters.", nameof(value));
            }
            _arguments = value;
        }
    }
    protected object? Result { get; set; }
    internal abstract Type ResultType { get; }

    internal SolutionMethod(MethodInfo method, object solutionContainer, Predicate<MethodInfo> validator)
    {
        _isValid = validator;
        RunValidationLogic(method);

        _solutionContainer = solutionContainer;
        _method = method;
    }
    protected void RunValidationLogic(MethodInfo method)
    {
        if (_isValid is null) throw new NotImplementedException($"Validation logic is not implemented/provided for the {this.GetType()}", new NullReferenceException());
        if (!_isValid(method)) throw new InvalidOperationException($"Wrong method was identified as {this.GetType()}");
    }

    internal object? Invoke()
    {
        var rawResult = _method.Invoke(_solutionContainer, Arguments);
        ProcessResult(rawResult);
        return Result;
    }
    protected abstract void ProcessResult(object? rawResult);
}
