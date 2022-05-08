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

using CCEasy.Services;
using CCEasy.Services.ArgumentsProcessing;
using System.Reflection;

namespace CCEasy.SolutionMethods;

/// <summary>
/// The base class that is inherited by concrete solution method implementations.
/// </summary>
/// <remarks>
/// Concrete implementations define different means of retrieving the result from the solution method.
/// </remarks>
internal abstract class SolutionMethod
{
    readonly object _solutionContainer;
    readonly protected MethodInfo _method;

    protected object?[]? Arguments { get; private set; }

    /// <summary>
    /// Binds the <see cref="SolutionMethod"/> to its actual <paramref name="method"/> implementation
    /// and the <paramref name="solutionContainer"/> instance of the type in which it's defined.
    /// </summary>
    /// <param name="method">The actual method definition.</param>
    /// <param name="solutionContainer">The instance of the type in which the <paramref name="method"/> is defined.</param>
    protected SolutionMethod(MethodInfo method, object solutionContainer)
    {
        _solutionContainer = solutionContainer;
        _method = method;
    }

    internal TResult Invoke<TResult, TInterpreted>(object?[]? arguments, Func<string, TInterpreted> interpreter)
    {
        Arguments = new ArgumentsProcessor<TInterpreted>(_method, arguments, interpreter).Process();
        var methodInfoReturnValue = _method.Invoke(_solutionContainer, Arguments);

        return TypeBinder.Cast<TResult>(RetrieveSolutionResult(methodInfoReturnValue));
    }

    protected abstract object? RetrieveSolutionResult(object? methodInfoResult);
}
