using System.Reflection;

namespace CCHelper;

internal abstract class SolutionMethod<TResult>
{
    readonly object _solutionContainer;
    readonly protected MethodInfo _method;
    object[]? _arguments;

    protected object[]? Arguments
    {
        get => _arguments;
        set => _arguments = new ArgumentsProcessor(_method, value).Process();
    }
    TResult? Result { get; set; }

    protected SolutionMethod(MethodInfo method, object solutionContainer, Predicate<MethodInfo> validator)
    {
        Validate(method, validator);

        _solutionContainer = solutionContainer;
        _method = method;
    }
    void Validate(MethodInfo method, Predicate<MethodInfo> validator)
    {
        if (validator is null) throw new NotImplementedException($"Validation logic is not implemented/provided" +
            $" for the {this.GetType()}", new NullReferenceException());
        if (!validator(method)) throw new InvalidOperationException($"Wrong method was identified as {this.GetType()}");
    }

    internal TResult? Invoke(object[] arguments)
    {
        Arguments = arguments;
        var rawResult = _method.Invoke(_solutionContainer, Arguments);
        SetResult(rawResult);
        return Result;
    }

    protected void SetResult(object? rawResult)
    {
        var result = RetrieveSolutionMethodSpecificResult(rawResult);
        ValidateResult(result);

        Result = (TResult)result!;
    }
    protected abstract object? RetrieveSolutionMethodSpecificResult(object? rawResult);
    void ValidateResult(object? result)
    {
        if (result is null) throw new ArgumentNullException(nameof(result), "Result can't be null.");
        if (result.GetType() != typeof(TResult))
        {
            throw new InvalidCastException($"Type provided in place of {nameof(TResult)} ({typeof(TResult)})" +
                $" doesn't match the actual type of the solution method ({result.GetType()})");
        }
    }

    class ArgumentsProcessor
    {
        const object[]? EMPTY_ARGUMENTS = null;

        readonly MethodInfo _method;
        readonly object?[]? _arguments;

        internal ArgumentsProcessor(MethodInfo method, object?[]? arguments)
        {
            _method = method;
            _arguments = arguments;
        }

        internal object[]? Process()
        {
            if (NoArgumentsPassed) return EMPTY_ARGUMENTS;

            ForbidNullArguments();
            ValidateNumberOfArguments();
            ValidateTypes();
            return _arguments!;
        }
        // arguments.Length == 0 when a client passes no arguments to `params object[]` parameter of the tester interface.
        bool NoArgumentsPassed => _arguments?.Length == 0;

        void ForbidNullArguments()
        {
            if (_arguments is null || _arguments.Any(argument => argument is null))
            {
                throw new ArgumentNullException("argument", "null arguments are currently not supported.");
            }
        }

        void ValidateNumberOfArguments()
        {
            if (_arguments!.Length != _method.GetParameters().Length)
            {
                throw new TargetParameterCountException("Number of arguments doesn't match the number of parameters.");
            }
        }

        void ValidateTypes()
        {
            foreach (var parameter in _method.GetParameters())
            {
                var correspondingArgumentType = _arguments![parameter.Position]!.GetType();
                if (!parameter.ParameterType.IsAssignableFrom(correspondingArgumentType))
                {
                    throw new ArgumentException($"Parameter [{parameter.ParameterType}] `{parameter.Name}` can't" +
                        $" be assigned the value of type [{correspondingArgumentType}] of the corresponding argument.");
                }
            }
        }
    }
}
