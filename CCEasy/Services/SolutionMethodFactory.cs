using CCEasy.SolutionMethods;
using System.Reflection;

namespace CCEasy.Services;

internal static class SolutionMethodFactory
{
    internal static SolutionMethod<TResult> Create<TSolutionContainer, TResult>(MethodInfo solutionMethodInfo)
        where TSolutionContainer : new()
    {
        var solutionContainer = new TSolutionContainer();
        if (solutionMethodInfo.HasSolutionLabel()) return new OutputSolution<TResult>(solutionMethodInfo, solutionContainer);
        if (solutionMethodInfo.HasResultLabel()) return new InputSolution<TResult>(solutionMethodInfo, solutionContainer);
        throw new ApplicationException("Discovered solution method doesn't match with any known implementation.");
    }
}
