using CCHelper.Core;
using System.Reflection;

namespace CCHelper.Services;

internal static class SolutionMethodFactory
{
    internal static SolutionMethod<TResult> Create<TResult>(MethodInfo solutionMethodInfo, object solutionContainer)
    {
        if (solutionMethodInfo.HasSolutionLabel()) return new OutputSolution<TResult>(solutionMethodInfo, solutionContainer);
        if (solutionMethodInfo.HasResultLabel()) return new InputSolution<TResult>(solutionMethodInfo, solutionContainer);
        throw new ApplicationException("Discovered solution method doesn't match with any known implementation.");
    }
}
