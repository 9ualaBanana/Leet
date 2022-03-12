using System.Reflection;
using CCHelper.Services.ArgumentsProcessing.ArgumentsFormats;

namespace CCHelper.Services.ArgumentsProcessing;

internal class ArgumentsProcessor
{
    const object[]? EMPTY_ARGUMENTS = null;
    readonly static ArgumentsFormat[] _supportedFormats =
    {
        new UnwrappedArgumentsFormat(),
        new StringSequenceArgumentsFormat()
    };

    readonly MethodInfo _method;
    readonly object?[] _arguments;

    internal ArgumentsProcessor(MethodInfo method, object?[]? arguments)
    {
        _arguments = NormalizedArguments(arguments);
        _method = method;
    }
    static object?[] NormalizedArguments(object?[]? arguments)
    {
        foreach (var argumentsFormat in _supportedFormats)
        {
            if (argumentsFormat.Match(arguments))
            {
                argumentsFormat.Normalize(ref arguments);
                break;
            }
        }
        return arguments!;
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