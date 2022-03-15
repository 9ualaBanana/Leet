using CCEasy.Services;
using CCEasy.Services.StringInterpreter;
using CCEasy.SolutionMethods;

namespace CCEasy;

/// <summary>
/// Provides the interface for exercising solution methods.
/// </summary>
/// <typeparam name="TSolutionContainer">The type where the solution method is defined.</typeparam>
/// <typeparam name="TResult">The result type of the solution method.</typeparam>
public class SolutionTester<TSolutionContainer, TResult> where TSolutionContainer : class, new()
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
    /// Instantiates the tester for the solution method defined inside <paramref name="solutionContainer"/>.
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
    /// <typeparam name="TInterpreted">The type of the elements inside the collection represented by <see cref="string"/> argument.</typeparam>
    /// <param name="expected">The expected result of the solution method.</param>
    /// <param name="interpreter">The delegate used for casting the elements inside the collection represented by <see cref="string"/> argument.</param>
    /// <param name="arguments">The arguments to the solution method being tested.</param>
    public void Test<TInterpreted>(object? expected, Func<string, TInterpreted> interpreter, params object?[]? arguments)
    {
        CollectionInStringInterpreter<TInterpreted>.TryInterpret(ref expected, interpreter);
        TypeBinder.CanBind(expected?.GetType(), typeof(TResult));
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
        var actual = _solutionMethod.Invoke(arguments, interpreter);

        _resultPresenter.DisplayResults(expected, actual);
    }

    public void SetResultOutput(Stream outputStream)
    {
        _resultPresenter.OutputStream = outputStream;
    }
}
