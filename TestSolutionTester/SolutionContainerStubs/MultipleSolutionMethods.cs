using CCHelper;

namespace TestSolutionTester;

internal class MultipleSolutionMethods
{
    [Solution]
    public int FirstSolutionMethod()
    {
        return 0;
    }
    public void SecondSolutionMethod([Result]int value)
    {
    }
}