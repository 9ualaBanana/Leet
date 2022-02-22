using System.Reflection;

namespace CCHelper;

internal static class SolutionMethodDiscovererFactory
{
    internal static SolutionMethod<TResult> SearchSolutionContainer<TResult>(object solutionContainer)
    {
        var singleSolutionMethod = solutionContainer.DiscoverSolutionMethod();
        if (singleSolutionMethod.IsOutputSolution()) return new OutputSolution<TResult>(singleSolutionMethod, solutionContainer);
        if (singleSolutionMethod.IsInputSolution()) return new InputSolution<TResult>(singleSolutionMethod, solutionContainer);
        throw new ApplicationException("Discovered solution method doesn't match with any known implementation.");
    }
    static MethodInfo DiscoverSolutionMethod(this object container)
    {
        return GetSingleSolutionInContainerOrThrow(container);
    }
    static MethodInfo GetSingleSolutionInContainerOrThrow(object container)
    {
        var validSolutionMethods = container.FindValidSolutionMethods();

        if (!validSolutionMethods.Any()) throw new EntryPointNotFoundException("Solution method was not found inside the provided solution container.");
        if (validSolutionMethods.Count() > 1) throw new AmbiguousMatchException("Solution container must contain exactly one solution method.");

        return validSolutionMethods.Single();
    }
}