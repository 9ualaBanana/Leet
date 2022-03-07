using CCHelper;

namespace TestSolutionTester;

internal class InputSolutionWrongType
{
    public int SolutionMethod([Result]int value)
    {
        return 1;
    }
    [Solution]
    internal void Ignored() { }
}