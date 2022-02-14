using CCHelper;
using System;

namespace TestSolutionTester;

internal static class SolutionContainerProvider
{
    internal static Action GetConstructor<TSolutionContainer>() where TSolutionContainer : class, new()
    {
        return () => new SolutionTester<TSolutionContainer>();
    }
}