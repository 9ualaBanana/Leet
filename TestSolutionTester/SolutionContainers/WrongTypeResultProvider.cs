using CCHelper;

namespace TestSolutionTester;

internal class WrongTypeResultProvider
{
    public int SolutionMethod([Result]int value)
    {
        return 1;
    }
    [Solution]
    internal void Ignored() { }
}