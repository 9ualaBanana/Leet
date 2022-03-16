using System.Reflection;

namespace CCEasy.Services;

internal static class SolutionMethodDiscoverer
{
    internal static MethodInfo SearchSolutionContainer<TSolutionContainer>()
    {
        return DiscoverSolutionMethod<TSolutionContainer>();
    }
    static MethodInfo DiscoverSolutionMethod<TSolutionContainer>()
    {
        return GetSingleSolutionInContainerOrThrow<TSolutionContainer>();
    }
    static MethodInfo GetSingleSolutionInContainerOrThrow<TSolutionContainer>()
    {
        var validSolutionMethods = FindValidSolutionMethods<TSolutionContainer>();

        if (!validSolutionMethods.Any())
        {
            throw new EntryPointNotFoundException("Solution method was not found inside the provided solution container.");
        }
        if (validSolutionMethods.Count() > 1)
        {
            throw new AmbiguousMatchException("Solution container must contain exactly one solution method.");
        }

        return validSolutionMethods.Single();
    }
    static IEnumerable<MethodInfo> FindValidSolutionMethods<TSolutionContainer>()
    {
        return typeof(TSolutionContainer).GetMethods().Where(SolutionMethodValidator.IsValidSolutionMethod);
    }
}