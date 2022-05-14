namespace Leet.Test.Framework.TestData;

public class HasSingleSolutionMethod
{
    [Solution]
    public int Solution() { return default; }
}

public class HasNoSolutionMethods
{
}

public class HasMultipleSolutionMethods
{
    [Solution]
    public int SolutionOne() { return default; }
    public void SolutionTwo([Result] int result) { }
}
