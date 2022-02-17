using CCHelper;
using System;

namespace TestSolutionTester;

internal static class SolutionTesterConstructorProvider
{
    internal static Func<SolutionTester<TSolutionContainer>> For<TSolutionContainer>() where TSolutionContainer : class, new()
    {
        return () => new SolutionTester<TSolutionContainer>();
    }
}