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
using CCEasy.Services.StringInterpreter;
using CCEasy.SolutionMethods;
using System.Reflection;

namespace CCEasy;

/// <summary>
/// Provides the interface for exercising solution methods.
/// </summary>
/// <typeparam name="TSolutionContainer">The type where the solution method is defined.</typeparam>
/// <typeparam name="TResult">The result type of the solution method.</typeparam>
public class SolutionTester<TSolutionContainer, TResult> where TSolutionContainer : new()
{
    readonly MethodInfo _solutionMethodInfo;
    readonly SolutionResultPresenter _resultPresenter;

    /// <summary>
    /// Instantiates the tester for a solution method defined inside <typeparamref name="TSolutionContainer"/>.
    /// </summary>
    /// <remarks>
    /// The solution method must have a public access modifier.
    /// </remarks>
    public SolutionTester()
    {
        _solutionMethodInfo = SolutionMethodDiscoverer.SearchSolutionContainer<TSolutionContainer>();
        _resultPresenter = new();
    }

    /// <summary>
    /// The interface for exercising the associated solution method.
    /// </summary>
    /// <param name="expected">The expected result of solution method.</param>
    /// <param name="arguments">The arguments to the solution method being tested.</param>
    public void Test(TResult expected, params object?[]? arguments)
    {
        Test(expected, int.Parse, arguments);
    }

    /// <summary>
    /// The interface for exercising the associated solution method.
    /// </summary>
    /// <param name="expected">The expected result of solution method.</param>
    /// <param name="arguments">The arguments to the solution method being tested.</param>
    public void Test(object? expected, params object?[]? arguments)
    {
        Test(expected, int.Parse, arguments);
    }

    /// <summary>
    /// The interface for exercising the associated solution method.
    /// </summary>
    /// <typeparam name="TInterpreted">The type of the elements inside the collection represented by <see cref="string"/> argument.</typeparam>
    /// <param name="expected">The expected result of the solution method.</param>
    /// <param name="interpreter">The delegate used for casting the elements inside the collection represented by <see cref="string"/> argument.</param>
    /// <param name="arguments">The arguments to the solution method being tested.</param>
    public void Test<TInterpreted>(object? expected, Func<string, TInterpreted> interpreter, params object?[]? arguments)
    {
        CollectionInStringInterpreter<TInterpreted>.TryInterpret(ref expected, interpreter);
        SolutionMethod<TResult>.EnsureResultTypeCompatibility(expected?.GetType(), "expectedResult");
        Test((TResult)expected!, interpreter, arguments);
    }

    /// <summary>
    /// The interface for exercising the associated solution method.
    /// </summary>
    /// <typeparam name="TInterpreted">The type of the elements inside the collection represented by <see cref="string"/> argument.</typeparam>
    /// <param name="expected">The expected result of the solution method.</param>
    /// <param name="interpreter">The delegate used for casting the elements inside the collection represented by <see cref="string"/> argument.</param>
    /// <param name="arguments">The arguments to the solution method being tested.</param>
    public void Test<TInterpreted>(TResult expected, Func<string, TInterpreted> interpreter, params object?[]? arguments)
    {
        var solutionMethod = SolutionMethodFactory.Create<TSolutionContainer, TResult>(_solutionMethodInfo);

        var actual = solutionMethod.Invoke(arguments, interpreter);

        _resultPresenter.DisplayResults(expected, actual);
    }

    public void SetResultOutput(Stream outputStream)
    {
        _resultPresenter.OutputStream = outputStream;
    }
}
