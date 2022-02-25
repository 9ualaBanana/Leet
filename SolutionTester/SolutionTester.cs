namespace CCHelper;

/// <summary>
/// Provides an interface for testing solution methods.
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
    /// Instantiates the tester for a solution method defined inside <paramref name="solutionContainer"/>.
    /// </summary>
    /// <remarks>
    /// The solution method must have a public access modifier.
    /// </remarks>
    /// <param name="solutionContainer">the instance of <typeparamref name="TSolutionContainer"/> where the solution method is defined.</param>
    public SolutionTester(TSolutionContainer solutionContainer)
    {
        _solutionMethod = SolutionMethodDiscovererFactory.SearchSolutionContainer<TResult>(solutionContainer);
    }

    public void Test(TResult expectedResult, params object[] arguments)
    {
        var actualResult = _solutionMethod.Invoke(arguments);

        new SolutionResultPresenter(expectedResult!, actualResult!).DisplayResults();
    }
}
