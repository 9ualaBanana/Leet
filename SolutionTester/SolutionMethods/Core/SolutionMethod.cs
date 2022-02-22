using System.Reflection;

namespace CCHelper;

/// <summary>
/// The base class that is inherited by concrete solution method implementations.
/// </summary>
/// <typeparam name="TResult">type of the solution result.</typeparam>
internal abstract class SolutionMethod<TResult>
{
    readonly object _solutionContainer;
    object[]? _arguments;

    readonly protected MethodInfo _method;
    protected object[]? Arguments
    {
        get => _arguments;
        set => _arguments = new ArgumentsProcessor(_method, value).Process();
    }

    protected SolutionMethod(MethodInfo method, object solutionContainer, Predicate<MethodInfo> validator)
    {
        Validate(method, validator);

        _solutionContainer = solutionContainer;
        _method = method;
    }
    void Validate(MethodInfo method, Predicate<MethodInfo> validator)
    {
        Guard.Against.Null(validator,
            nameof(validator), $"Validation logic is not implemented/provided for {this.GetType()}.");

        if (!validator(method))
        {
            throw new InvalidOperationException($"Wrong method was identified as {this.GetType()}");
        }
    }

    internal TResult Invoke(object[] arguments)
    {
        Arguments = arguments;
        var methodInfoResult = _method.Invoke(_solutionContainer, Arguments);
        var result = RetrieveSolutionMethodSpecificResult(methodInfoResult);
        return ValidateResult(result);
    }

    protected abstract object? RetrieveSolutionMethodSpecificResult(object? methodInfoResult);
    static TResult ValidateResult(object? result)
    {
        Guard.Against.Null(result, nameof(result));
        Guard.Against.ResultTypeMismatch<TResult>(result, nameof(result));

        return (TResult)result;
    }

    class ArgumentsProcessor
    {
        const object[]? EMPTY_ARGUMENTS = null;

        readonly MethodInfo _method;
        object[] _arguments;

        internal ArgumentsProcessor(MethodInfo method, object?[]? arguments)
        {
            _arguments = Guard.Against.NullArguments(arguments, nameof(arguments));
            _method = method;
        }

        internal object[]? Process()
        {
            if (NoArgumentsWerePassed) return EMPTY_ARGUMENTS;

            ValidateArguments();
            return _arguments;
        }
        bool NoArgumentsWerePassed => _arguments.Length == 0;

        void ValidateArguments()
        {
            ValidateArgumentsFormat();
            ValidateArgumentsNumber();
            ValidateArgumentsTypes();
        }

        void ValidateArgumentsFormat()
        {
            if (ArgumentsUnwrapped) FixArgumentsFormat();
        }
        /// <summary>
        /// The argument passed as a jagged array got unwrapped by <c>`params object[]`</c>.
        /// </summary>
        /// <remarks>
        /// Corrupts the actual number of passed arguments.
        /// </remarks>
        bool ArgumentsUnwrapped => _arguments.GetType() != typeof(object[]);
        /// <summary>
        /// Wraps arguments back in <c>object[]</c>.
        /// </summary>
        /// <remarks>
        /// Results in the correct number of arguments.
        /// </remarks>
        void FixArgumentsFormat()
        {
            _arguments = new object[] { _arguments! };
        }

        void ValidateArgumentsNumber()
        {
            if (_arguments.Length != _method.GetParameters().Length)
            {
                throw new TargetParameterCountException("Number of arguments doesn't match the number of parameters.");
            }
        }

        void ValidateArgumentsTypes()
        {
            foreach (var parameter in _method.GetParameters())
            {
                var correspondingArgumentType = _arguments[parameter.Position].GetType();
                if (!correspondingArgumentType.IsAssignableTo(parameter.ParameterType))
                {
                    throw new ArgumentException($"Parameter [{parameter.ParameterType}] `{parameter.Name}` can't" +
                        $" be assigned the value of type [{correspondingArgumentType}] of the corresponding argument.");
                }
            }
        }
    }
}
