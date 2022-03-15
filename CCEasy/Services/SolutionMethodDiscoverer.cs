using CCEasy.SolutionMethods;
using System.Reflection;

namespace CCEasy.Services;

internal static class SolutionMethodDiscoverer
{
    internal static SolutionMethod<TResult> SearchSolutionContainer<TResult>(object solutionContainer)
    {
        var singleSolutionMethod = solutionContainer.DiscoverSolutionMethod<TResult>();
        return SolutionMethodFactory.Create<TResult>(singleSolutionMethod, solutionContainer);
    }
    static MethodInfo DiscoverSolutionMethod<TResult>(this object container)
    {
        return GetSingleSolutionInContainerOrThrow<TResult>(container);
    }
    static MethodInfo GetSingleSolutionInContainerOrThrow<TResult>(object container)
    {
        var validSolutionMethods = container.FindValidSolutionMethods();

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
    static IEnumerable<MethodInfo> FindValidSolutionMethods(this object container)
    {
        return container.GetType().GetMethods().Where(SolutionMethodValidator.IsValidSolutionMethod);
    }
}