//MIT License

//Copyright (c) 2022 GualaBanana

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Reflection;
using CCEasy.Services.StringInterpreter;

namespace CCEasy.Services.ArgumentsProcessing;

internal class ArgumentsProcessor<TInterpreted>
{
    const object[]? EMPTY_ARGUMENTS = null;

    readonly MethodInfo _method;
    readonly object?[] _arguments;
    readonly Func<string, TInterpreted> _interpreter;

    internal ArgumentsProcessor(MethodInfo method, object?[]? arguments, Func<string, TInterpreted> interpreter)
    {
        _arguments = Preprocess(arguments);
        _method = method;
        _interpreter = interpreter;
    }
    static object?[] Preprocess(object?[]? arguments)
    {
        if (ArgumentsUnwrapped(arguments)) WrapArguments(ref arguments);

        return arguments!;
    }

    /// <summary>
    /// The single <c>null</c> argument or a jagged array argument got unwrapped by <c>params object[]</c>.
    /// </summary>
    static bool ArgumentsUnwrapped(object?[]? arguments)
    {
        return arguments is null || arguments.GetType() != typeof(object[]);
    }

    /// <summary>
    /// Restores the actual number of passed arguments.
    /// </summary>
    static void WrapArguments(ref object?[]? arguments) => arguments = new object?[] { arguments };

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
            ref var correspondingArgument = ref _arguments[parameter.Position];
            if (TypeBinder.ArgumentCanBindToParameter(correspondingArgument, parameter)) continue;

            CollectionInStringInterpreter<TInterpreted>.TryInterpret(ref correspondingArgument, _interpreter);

            if (TypeBinder.ArgumentCanBindToParameter(correspondingArgument, parameter)) continue;

            var argumentInfo = correspondingArgument is null ? "null" : $"<{correspondingArgument.GetType()}>";
            var parameterInfo = $"{parameter.Name} <{parameter.ParameterType}>";
            throw new ArgumentException($"The argument [{argumentInfo}] can't bind to the parameter [{parameterInfo}]");
        }
    }
}