namespace CCHelper;

public class SolutionTester<TSolutionContainer> where TSolutionContainer : class, new()
{
    readonly TSolutionContainer _solutionContainer;
    readonly SolutionMethod _solutionMethod;

    object? _actualResult;
    object? _expectedResult;

    public SolutionTester() : this(new()) { }

    public SolutionTester(TSolutionContainer solutionContainer)
    {
        _solutionContainer = solutionContainer;
        _solutionMethod = SolutionMethodDiscovererFactory.SearchSolutionContainer(_solutionContainer);
    }
    public void Test(object expectedResult, params object[] arguments)
    {
        _expectedResult = expectedResult;
        _solutionMethod.Arguments = arguments;
        _actualResult = _solutionMethod.Invoke();

        ResolveResultsTypes();
        OutputResults();
    }
    void ResolveResultsTypes()
    {
        _expectedResult = Convert.ChangeType(_expectedResult, _solutionMethod.ResultType);
        _actualResult = Convert.ChangeType(_actualResult, _solutionMethod.ResultType);
    }
    void OutputResults()
    {
        Console.WriteLine($"Expected result: {_expectedResult}");
        Console.WriteLine($"Actual result: {_actualResult}");
    }
}
