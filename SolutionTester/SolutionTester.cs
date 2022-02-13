namespace CCHelper;

public class SolutionTester<TSolution> where TSolution : class, new()
{
    readonly TSolution _solution;
    readonly SolutionMethod _solutionMethod;

    object _actualResult;
    object _expectedResult;

    public SolutionTester()
    {
        _solution = new();
        _solutionMethod = SolutionMethodFactory.SearchSolutionContainer(_solution);
    }

    public void Test(object[] arguments, object expectedResult)
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
