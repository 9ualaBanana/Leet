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

using Leet.Services;
using System.Reflection;

namespace Leet;

/// <summary>
/// Wraps the solution method to provide ways for obtaining its result based on the return type and label.
/// </summary>
internal class SolutionMethod
{
    /// <summary>
    /// This object is required to call invoke on the method it contains.
    /// </summary>
    readonly object _solutionContainer;
    /// <summary>
    /// The wrapped method.
    /// </summary>
    readonly protected MethodInfo _method;

    protected object?[]? Arguments { get; private set; }

    /// <summary>
    /// Binds the <see cref="SolutionMethod"/> to its actual <paramref name="method"/> implementation
    /// and the <paramref name="solutionContainer"/> instance of the type in which it's defined.
    /// </summary>
    /// <param name="method">The actual method definition.</param>
    /// <param name="solutionContainer">The instance of the type in which the <paramref name="method"/> is defined.</param>
    internal SolutionMethod(MethodInfo method, object solutionContainer)
    {
        _solutionContainer = solutionContainer;
        _method = method;
    }

    internal TResult Invoke<TResult, TInterpreted>(object?[]? arguments, Func<string, TInterpreted> interpreter)
    {
        Arguments = new ArgumentsProcessor<TInterpreted>(_method, arguments, interpreter).Process();
        var methodInfoReturnValue = _method.Invoke(_solutionContainer, Arguments);

        return TypeBinder.Cast<TResult>(ObtainSolutionResult(methodInfoReturnValue));
    }

    /// <summary>
    /// Obtains the solution result based on the solution result type and label.
    /// </summary>
    /// <param name="methodInfoResult">The return value from the <see cref="_method"/> invocation.</param>
    /// <returns>The actual solution result.</returns>
    object? ObtainSolutionResult(object? methodInfoResult)
    {
        if (_method.ReturnType != typeof(void)) return methodInfoResult;

        var resultParameter = _method.GetParameters().Single(parameter => parameter.IsDefined(typeof(ResultAttribute)));
        return Arguments![resultParameter.Position];
    }

    /// <summary>
    /// Predicate defining the correct properties for methods to be considered valid solution methods.
    /// </summary>
    /// <remarks>
    /// Throws if <paramref name="methodInfo"/> has conflicting/incorrect properties characteristic of solution methods.
    /// </remarks>
    /// <param name="methodInfo"><see cref="MethodInfo"/> to check for validity.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="methodInfo"/> is a valid solution method;
    /// <c>false</c> if it's a regular method.
    /// </returns>
    /// <exception cref="AmbiguousMatchException">
    /// <paramref name="methodInfo"/> is labeled with multiple attributes.
    /// </exception>
    /// <exception cref="FormatException">
    /// The return type of <paramref name="methodInfo"/> is not appropriate
    /// for the attribute with which the method is labeled.
    /// </exception>
    internal static bool IsValid(MethodInfo methodInfo)
    {
        bool hasSolutionLabel = methodInfo.IsDefined(typeof(SolutionAttribute));
        int resultLabelsCount = methodInfo.GetParameters().
            Where(parameter => parameter.CustomAttributes.
            Any(attribute => attribute.AttributeType == typeof(ResultAttribute))).Count();
        var returnType = methodInfo.ReturnType;

        if (hasSolutionLabel && resultLabelsCount > 0)
            throw new AmbiguousMatchException("Solution method must be labeled with one type of attribute.");
        if (resultLabelsCount > 1)
            throw new AmbiguousMatchException($"Multiple {nameof(ResultAttribute)} attributes are not allowed.");
        if (resultLabelsCount > 0 && returnType != typeof(void))
            throw new FormatException($"Method labeled with {nameof(ResultAttribute)} must return void.");
        if (hasSolutionLabel && returnType == typeof(void))
            throw new FormatException($"Method labeled with {nameof(SolutionAttribute)} can't return void.");

        return hasSolutionLabel || resultLabelsCount == 1;
    }
}
