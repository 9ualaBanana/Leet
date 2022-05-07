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
/// <typeparam name="TResult">The type of the solution result.</typeparam>
internal abstract class SolutionMethod<TResult>
{
    readonly object _solutionContainer;
    readonly protected MethodInfo _method;

    protected object?[]? Arguments { get; private set; }
    protected abstract Type ResultType { get; }

    /// <summary>
    /// Contains all the necessary logic for ensuring the proper construction of derived classes.<br/>
    /// The constructors of concrete <see cref="SolutionMethod{TResult}"/> implementations must be used 
    /// only for calling this base constructor.<br/>
    /// The actual specific construction logic must be defined inside <see cref="Init"/>.
    /// </summary>
    /// <param name="method">The actual method definition.</param>
    /// <param name="solutionContainer">The instance of the type in which the <paramref name="method"/> is defined.</param>
    protected SolutionMethod(MethodInfo method, object solutionContainer)
    {
        _solutionContainer = solutionContainer;
        _method = method;
        Init();
        EnsureResultTypeCompatibility(ResultType, nameof(ResultType));
    }

    /// <summary>
    /// Derived classes must provide their construction logic in this method instead of the actual constructor.
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// The constraint that satisfies both reference and nullable types doesn't exist.<br/>
    /// This method checks at runtime that <typeparamref name="TResult"/> conforms to these constraints.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    static internal void EnsureResultTypeCompatibility(Type? type, string resultName)
    {
        if (!TypeBinder.CanBind(type, typeof(TResult)))
        {
            var typeInfo = type is null ? "[null]" : $"<{type}>";
            throw new ArgumentException($"The solution result {typeInfo} " +
                $"is not compatible with the provided type parameter <{typeof(TResult)}>.", resultName);
        }
    }

    internal TResult? Invoke<TInterpreted>(object?[]? arguments, Func<string, TInterpreted> interpreter)
    {
        Arguments = new ArgumentsProcessor<TInterpreted>(_method, arguments, interpreter).Process();
        var methodInfoReturnValue = _method.Invoke(_solutionContainer, Arguments);

        // EnsureResultTypeCompatibility() justifies the usage of the null-forgiving operator.
        return (TResult)RetrieveSolutionResult(methodInfoReturnValue)!;
    }

    protected abstract object? RetrieveSolutionResult(object? methodInfoResult);
}
