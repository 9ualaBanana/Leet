using CCEasy.Services;
using CCEasy.Test.Framework.Abstractions.SolutionContext;
using CCEasy.Test.Framework.Abstractions.SolutionMethod;
using CCEasy.Test.Framework.TestData;
using System;
using System.Reflection;
using Xunit;

namespace CCEasy.Test.Tests.Components;

public class TestSolutionMethodDiscoverer : DynamicContextFixture
{
    Action SUT_SearchSolutionContainer<TResult>(TResult _)
    {
        return () => SolutionMethodDiscoverer.SearchSolutionContainer<TResult>(_context.SolutionContainer.Instance);
    }



    [Fact]
    public void SUT_SolutionMethodInSolutionContainer_DoesNotThrow()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_SearchSolutionContainer(TypeData.DummyValue)));
    }

    [Fact]
    public void SUT_NoSolutionMethodInSolutionContainer_Throws()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<EntryPointNotFoundException>(SUT_SearchSolutionContainer(TypeData.DummyType));
    }

    [Fact]
    public void SUT_MultipleSolutionMethodsInSolutionContainer_Throws()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(TypeData.DummyType)
            .PutInContext(_context);
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SUT_SearchSolutionContainer(TypeData.DummyType));
    }
}
