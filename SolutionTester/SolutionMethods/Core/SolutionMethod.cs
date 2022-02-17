using System.Reflection;

namespace CCHelper;

internal abstract class SolutionMethod
{
    readonly object _solutionContainer;
    readonly protected MethodInfo _method;
    object[]? _arguments;
    protected Type? _resultType;

    internal object[]? Arguments
    {
        get => _arguments;
        set => _arguments = _argumentsProcessor.Process(value);
    }
    ArgumentsProcessor _argumentsProcessor;
    protected object? Result { get; set; }
    internal abstract Type ResultType { get; }

    protected SolutionMethod(MethodInfo method, object solutionContainer, Predicate<MethodInfo> validator)
    {
        Validate(method, validator);

        _solutionContainer = solutionContainer;
        _method = method;
        _argumentsProcessor = new(method);
    }
    void Validate(MethodInfo method, Predicate<MethodInfo> validator)
    {
        if (validator is null) throw new NotImplementedException($"Validation logic is not implemented/provided for the {this.GetType()}", new NullReferenceException());
        if (!validator(method)) throw new InvalidOperationException($"Wrong method was identified as {this.GetType()}");
    }

    internal object? Invoke()
    {
        var rawResult = _method.Invoke(_solutionContainer, Arguments);
        ProcessResult(rawResult);
        return Result;
    }
    protected abstract void ProcessResult(object? rawResult);

    class ArgumentsProcessor
    {
        readonly MethodInfo _method;
        object[] _arguments;

        internal ArgumentsProcessor(MethodInfo method)
        {
            _method = method;
        }

        internal object[]? Process(object[]? arguments)
        {
            if (arguments is null || arguments.Length == 0) return null;

            _arguments = arguments;
            ValidateTypeCompatibilityBetweenArgumentsAndParameters();
            return _arguments;
        }


        void ValidateTypeCompatibilityBetweenArgumentsAndParameters()
        {
            foreach (var parameter in _method.GetParameters())
            {
                var correspondingArgumentType = _arguments[parameter.Position]?.GetType();
                try
                {
                    Convert.ChangeType(_arguments[parameter.Position], parameter.ParameterType);
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException($"Parameter [{parameter.ParameterType}] `{parameter.Name}` can't be assigned the value of type [{correspondingArgumentType}] of the corresponding argument.");
                }
            }
        }
    }
}
