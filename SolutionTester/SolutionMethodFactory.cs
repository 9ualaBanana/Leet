namespace CCHelper;

internal static class SolutionMethodFactory
{
    internal static SolutionMethod SearchSolutionContainer(object solutionContainer)
    {
        var solutionMethod = solutionContainer.GetType().GetMethods().DetectSolutionMethod();
        if (solutionMethod.HasSolutionAttribute()) return new ReturnValueSolutionMethod(solutionMethod, solutionContainer);
        if (solutionMethod.HasResultAttribute()) return new ResultArgumentSolutionMethod(solutionMethod, solutionContainer);
        throw new ApplicationException("Something went wrong when trying to detect the solution method.");
    }

}