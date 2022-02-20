namespace CCHelper;

public class SolutionTester<TSolutionContainer, TResult> where TSolutionContainer : class, new()
{
    readonly SolutionMethod<TResult> _solutionMethod;

    TResult? _actualResult;
    TResult? _expectedResult;

    public SolutionTester() : this(new TSolutionContainer())
    {
    }

    public SolutionTester(TSolutionContainer solutionContainer)
    {
        _solutionMethod = SolutionMethodDiscovererFactory.SearchSolutionContainer<TResult>(solutionContainer);
    }

    public void Test(TResult expectedResult, params object[] arguments)
    {
        _expectedResult = expectedResult;
        _actualResult = _solutionMethod.Invoke(arguments);

        OutputResults();
    }
    void OutputResults()
    {
        Console.WriteLine($"Expected result: {_expectedResult}");
        Console.WriteLine($"Actual result: {_actualResult}");
    }
}
