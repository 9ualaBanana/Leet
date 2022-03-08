using System.Reflection;

namespace CCHelper.Services;

internal class ArgumentsProcessor
{
    const object[]? EMPTY_ARGUMENTS = null;

    readonly MethodInfo _method;
    readonly object?[] _arguments;

    internal ArgumentsProcessor(MethodInfo method, object?[]? arguments)
    {
        _arguments = ArgumentsInCorrectFormat(arguments);
        _method = method;
    }
    static object?[] ArgumentsInCorrectFormat(object?[]? arguments)
    {
        return ArgumentsMisinterpreted(arguments) ? FixArgumentsFormat(arguments) : arguments!;
    }
    /// <summary>
    /// The single <c>null</c> argument or the argument passed as a jagged array got unwrapped by <c>params object[]</c>.
    /// </summary>
    /// <remarks>
    /// Corrupts the actual number of passed arguments.
    /// </remarks>
    static bool ArgumentsMisinterpreted(object?[]? arguments)
    {
        return arguments is null || arguments.GetType() != typeof(object[]);
    }
    /// <summary>
    /// Wraps arguments back in <c>object[]</c>.
    /// </summary>
    /// <remarks>
    /// Results in the correct number of arguments.
    /// </remarks>
    static object?[] FixArgumentsFormat(object?[]? arguments)
    {
        return new object?[] { arguments };
    }

    internal object?[]? Process()
    {
        ValidateArguments();

        return NoArgumentsWerePassed ? EMPTY_ARGUMENTS : _arguments;
    }
    bool NoArgumentsWerePassed => _arguments.Length == 0;

    void ValidateArguments()
    {
        ValidateArgumentsNumber();
        ValidateArgumentsTypes();
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
            var correspondingArgument = _arguments[parameter.Position];
            if (!TypeBinder.ArgumentCanBindToParameter(correspondingArgument, parameter))
            {
                var argumentInfo = correspondingArgument is null ? "null" : $"<{correspondingArgument.GetType()}>";
                var parameterInfo = $"{parameter.Name} <{parameter.ParameterType}>";
                throw new ArgumentException($"The argument [{argumentInfo}] can't bind to the parameter [{parameterInfo}]");
            }
        }
    }
}