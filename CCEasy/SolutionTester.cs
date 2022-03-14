using CCEasy.Core;
using CCEasy.Services;

namespace CCEasy;

/// <summary>
/// Provides an interface for exercising solution methods.
/// </summary>
/// <typeparam name="TSolutionContainer">The type where the solution method is defined.</typeparam>
/// <typeparam name="TResult">The result type of the solution method.</typeparam>
public class SolutionTester<TSolutionContainer, TResult> where TSolutionContainer : new()
{
    readonly SolutionMethod<TResult> _solutionMethod;
    readonly SolutionResultPresenter _resultPresenter;

    /// <summary>
    /// Instantiates the tester for a solution method defined inside <typeparamref name="TSolutionContainer"/>.
    /// </summary>
    /// <remarks>
    /// The solution method must have a public access modifier.
    /// </remarks>
    public SolutionTester() : this(new TSolutionContainer())
    {
    }

    /// <summary>
    /// Instantiates the tester for <see cref="SolutionMethod{TResult}"/> defined inside <paramref name="solutionContainer"/>.
    /// </summary>
    /// <remarks>
    /// The solution method must have a public access modifier.
    /// </remarks>
    /// <param name="solutionContainer">The instance of <typeparamref name="TSolutionContainer"/> where the solution method is defined.</param>
    public SolutionTester(TSolutionContainer solutionContainer)
    {
        _solutionMethod = SolutionMethodDiscoverer.SearchSolutionContainer<TResult>(solutionContainer);
        _resultPresenter = new();
    }

    /// <summary>
    /// <inheritdoc cref="Test{TInterpreted}(TResult, Func{string, TInterpreted}, object?[]?)"/>
    /// </summary>
    /// <param name="expected"><inheritdoc cref="Test{TInterpreted}(TResult, Func{string, TInterpreted}, object?[]?)"/></param>
    /// <param name="arguments"><inheritdoc cref="Test{TInterpreted}(TResult, Func{string, TInterpreted}, object?[]?)"/></param>
    public void Test(TResult expected, params object?[]? arguments)
    {
        Test(expected, int.Parse, arguments);
    }

    /// <summary>
    /// The interface for exercising the associated <see cref="SolutionMethod{TResult}"/>.
    /// </summary>
    /// <typeparam name="TInterpreted">The type of the elements inside the sequence represented by <see cref="string"/> argument.</typeparam>
    /// <param name="expected">The expected result of <see cref="SolutionMethod{TResult}"/>.</param>
    /// <param name="interpreter">The delegate used for casting the elements inside the sequence represented by <see cref="string"/> argument.</param>
    /// <param name="arguments">The arguments to the <see cref="SolutionMethod{TResult}"/> being tested.</param>
    public void Test<TInterpreted>(TResult expected, Func<string, TInterpreted> interpreter, params object?[]? arguments)
    {
        var actual = _solutionMethod.Invoke(arguments, interpreter);

        _resultPresenter.DisplayResults(expected, actual);
    }

    public void SetResultOutput(Stream outputStream)
    {
        _resultPresenter.OutputStream = outputStream;
    }
}
