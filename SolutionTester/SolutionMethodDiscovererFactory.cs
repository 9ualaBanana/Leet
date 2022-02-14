namespace CCHelper;

internal static class SolutionMethodDiscovererFactory
{
    internal static SolutionMethod SearchSolutionContainer(object solutionContainer)
    {
        var singleSolutionMethod = solutionContainer.DiscoverSolutionMethod();
        if (singleSolutionMethod.IsOutputSolution()) return new OutputSolution(singleSolutionMethod, solutionContainer);
        if (singleSolutionMethod.IsInputSolution()) return new InputSolution(singleSolutionMethod, solutionContainer);
        throw new ApplicationException("Something went wrong when trying to detect the solution method.");
    }
}