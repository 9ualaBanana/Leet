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
using Leet.Services.StringInterpreter;

namespace Leet;

/// <summary>
/// Provides the type-safe interface for exercising solution methods.
/// </summary>
/// <remarks>
/// Supports passing of <see cref="IEnumerable{T}"/> arguments as <see cref="string"/>s.
/// </remarks>
/// <typeparam name="TSolutionContainer">The type where the solution method is defined.</typeparam>
/// <typeparam name="TResult">The result type of the solution method.</typeparam>
public class Solution<TSolutionContainer, TResult> where TSolutionContainer : new()
{
    SolutionMethod _solutionMethod = default!;
    readonly SolutionResultPresenter _resultPresenter;

    /// <summary>
    /// Instantiates the interface for testing the solution defined inside <typeparamref name="TSolutionContainer"/>.
    /// </summary>
    /// <remarks>
    /// The solution must have a public access modifier.
    /// </remarks>
    public Solution()
    {
        _resultPresenter = new();
    }

    /// <summary>
    /// Tests the solution with <paramref name="arguments"/> against <paramref name="expected"/> result.
    /// </summary>
    /// <remarks>
    /// <paramref name="arguments"/> are type-safe and resolved at runtime.
    /// </remarks>
    /// <param name="expected">The expected result of the solution.</param>
    /// <param name="arguments">The arguments to the solution.</param>
    public void Test(TResult expected, params object?[]? arguments)
    {
        Test(expected, int.Parse, arguments);
    }

    /// <summary>
    /// Tests the solution with <paramref name="arguments"/> against <paramref name="expected"/> result.
    /// </summary>
    /// <remarks>
    /// <paramref name="arguments"/> are type-safe and resolved at runtime.
    /// </remarks>
    /// <param name="expected">The expected result of the solution.</param>
    /// <param name="arguments">The arguments to the solution.</param>
    public void Test(object? expected, params object?[]? arguments)
    {
        Test(expected, int.Parse, arguments);
    }

    /// <summary>
    /// Tests the solution with <paramref name="arguments"/> against <paramref name="expected"/> result
    /// using <paramref name="interpreter"/> to interpret the elements inside the collection represented as <see cref="string"/>
    /// to which either of arguments refer.
    /// </summary>
    /// <remarks>
    /// <paramref name="arguments"/> are type-safe and resolved at runtime.
    /// </remarks>
    /// <typeparam name="TInterpreted">The type of the elements inside the collection represented by either of <see cref="string"/> arguments.</typeparam>
    /// <param name="expected">The expected result of the solution method.</param>
    /// <param name="interpreter">The delegate used for casting the elements inside the collection represented by either of <see cref="string"/> arguments.</param>
    /// <param name="arguments">The arguments to the solution method.</param>
    public void Test<TInterpreted>(object? expected, Func<string, TInterpreted> interpreter, params object?[]? arguments)
    {
        CollectionInStringInterpreter<TInterpreted>.TryInterpret(ref expected, interpreter);
        Test(TypeBinder.Cast<TResult>(expected), interpreter, arguments);
    }

    /// <summary>
    /// Tests the solution with <paramref name="arguments"/> against <paramref name="expected"/> result
    /// using <paramref name="interpreter"/> to interpret the elements inside the collection represented by <see cref="string"/>
    /// to which <paramref name="arguments"/> refer.
    /// </summary>
    /// <remarks>
    /// <paramref name="arguments"/> are type-safe and resolved at runtime.
    /// </remarks>
    /// <typeparam name="TInterpreted">The type of the elements inside the collection represented as <see cref="string"/> <paramref name="arguments"/>.</typeparam>
    /// <param name="expected">The expected result of the solution method.</param>
    /// <param name="interpreter">The delegate used for casting the elements inside the collection represented by <see cref="string"/> <paramref name="arguments"/>.</param>
    /// <param name="arguments">The arguments to the solution method.</param>
    public void Test<TInterpreted>(TResult expected, Func<string, TInterpreted> interpreter, params object?[]? arguments)
    {
        // Solution method is instantiated anew for each test run
        // to ensure its solution container's state is not invalidated by previous test runs.
        _solutionMethod = new(
            method: new SolutionMethodDiscoverer(typeof(TSolutionContainer)).DiscoverSolutionMethodInfo(),
            new TSolutionContainer()
            );

        var actual = _solutionMethod.Invoke<TResult, TInterpreted>(arguments, interpreter);

        _resultPresenter.DisplayResults(expected, actual);
    }

    public void SetResultOutput(Stream outputStream)
    {
        _resultPresenter.OutputStream = outputStream;
    }
}
