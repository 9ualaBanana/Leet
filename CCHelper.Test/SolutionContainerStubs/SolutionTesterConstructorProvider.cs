using CCHelper;
using System;

namespace TestSolutionTester;

internal static class SolutionTesterConstructorProvider
{
    internal static Func<SolutionTester<TSolutionContainer, TResult>> For<TSolutionContainer, TResult>() where TSolutionContainer : class, new()
    {
        return () => new SolutionTester<TSolutionContainer, TResult>();
    }
}