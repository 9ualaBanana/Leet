using CCHelper.Core;
using CCHelper.Services;

namespace CCHelper;

/// <summary>
/// Provides an interface for exercising solution methods.
/// </summary>
/// <typeparam name="TSolutionContainer">the type where the solution method is defined.</typeparam>
/// <typeparam name="TResult">the result type of the solution method.</typeparam>
public class SolutionTester<TSolutionContainer, TResult> where TSolutionContainer : class, new()
{
    readonly SolutionMethod<TResult> _solutionMethod;

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
    /// <param name="solutionContainer">the instance of <typeparamref name="TSolutionContainer"/> where the solution method is defined.</param>
    public SolutionTester(TSolutionContainer solutionContainer)
    {
        _solutionMethod = SolutionMethodDiscoverer.SearchSolutionContainer<TResult>(solutionContainer);
    }

    /// <summary>
    /// <inheritdoc cref="Test{TInterpreted}(TResult, Func{string, TInterpreted}, object?[]?)"/>
    /// </summary>
    /// <param name="expectedResult"><inheritdoc cref="Test{TInterpreted}(TResult, Func{string, TInterpreted}, object?[]?)"/></param>
    /// <param name="arguments"><inheritdoc cref="Test{TInterpreted}(TResult, Func{string, TInterpreted}, object?[]?)"/></param>
    public void Test(TResult expectedResult, params object?[]? arguments)
    {
        Test(expectedResult, int.Parse, arguments);
    }

    /// <summary>
    /// The interface for exercising the associated <see cref="SolutionMethod{TResult}"/>.
    /// </summary>
    /// <typeparam name="TInterpreted">the type of the elements inside the sequence represented by <see cref="string"/> argument.</typeparam>
    /// <param name="expectedResult">the expected result of <see cref="SolutionMethod{TResult}"/>.</param>
    /// <param name="interpreter">the delegate used for casting the elements inside the sequence represented by <see cref="string"/> argument.</param>
    /// <param name="arguments">the arguments to the <see cref="SolutionMethod{TResult}"/> being tested.</param>
    public void Test<TInterpreted>(TResult expectedResult, Func<string, TInterpreted> interpreter, params object?[]? arguments)
    {
        var actualResult = _solutionMethod.Invoke(arguments, interpreter);

        new SolutionResultPresenter(expectedResult!, actualResult!).DisplayResults();
    }
}
