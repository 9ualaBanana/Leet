using CCHelper.Services;
using CCHelper.Test.Framework;
using CCHelper.Test.Framework.Abstractions.SolutionContext;
using CCHelper.Test.Framework.Abstractions.SolutionMethod;
using CCHelper.Test.Framework.TestData;
using System;
using System.Reflection;
using Xunit;

namespace CCHelper.Test.Tests.Components;

public class TestSolutionMethodDiscoverer : DynamicContextFixture
{
    Action SUT_SearchSolutionContainer<TResult>(TResult _)
    {
        return () => SolutionMethodDiscoverer.SearchSolutionContainer<TResult>(_context.SolutionContainer.Instance);
    }

    [Fact]
    public void ShouldNotThrow_WhenSolutionMethodIsDiscovered()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.True(SUT_SearchSolutionContainer(TypeData.DummyValue).DoesNotThrow());
    }

    [Fact]
    public void ShouldThrowEntryPointNotFoundException_WhenSolutionContainerHasNoSolutionMethods()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<EntryPointNotFoundException>(SUT_SearchSolutionContainer(TypeData.DummyType));
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenSolutionContainerHasMultipleSolutionMethods()
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
