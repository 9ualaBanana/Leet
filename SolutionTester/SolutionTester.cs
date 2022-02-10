using System.Reflection;

namespace CCHelper;

public class SolutionTester<Solution> where Solution : new()
{
    readonly Solution _solution = new();
    MethodInfo? _solutionMethod;

    // These are null in case the solution method returns void.
    object? _actualResult;
    object? _expectedResult;

    MethodInfo _SolutionMethod
    {
        get
        {
            if (_solutionMethod is not null) return _solutionMethod;

            var methods = _solution.GetType().GetMethods().Where(method => Attribute.GetCustomAttribute(method, typeof(SolutionAttribute)) is not null);
            if (methods.Count() == 0) throw new MissingMethodException("None of the methods is labeled as the solution.");
            if (methods.Count() > 1) throw new AmbiguousMatchException("Exactly one method needs to be labeled as the solution.");
            return _solutionMethod = methods.ToList()[0];
        }
    }

    public void Test(object[] arguments, object expectedResult)
    {
        _expectedResult = expectedResult;
        _actualResult = _SolutionMethod.Invoke(_solution, arguments);
        _expectedResult = Convert.ChangeType(_expectedResult, _SolutionMethod.ReturnType);
        _actualResult = Convert.ChangeType(_actualResult, _SolutionMethod.ReturnType);
        OutputResults();
    }
    void OutputResults()
    {
        Console.WriteLine($"Expected result: {_expectedResult}");
        Console.WriteLine($"Actual result: {_actualResult}");
    }
}
