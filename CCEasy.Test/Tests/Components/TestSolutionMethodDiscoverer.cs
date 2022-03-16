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
    Action SUT_SearchSolutionContainer<TSolutionContainer>()
    {
        return () => SolutionMethodDiscoverer.SearchSolutionContainer<TSolutionContainer>();
    }



    [Fact]
    public void SearchSolutionContainer_WithSingleSolutionMethod_DoesNotThrow()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_SearchSolutionContainer<HasSingleSolutionMethod>));
    }

    [Fact]
    public void SearchSolutionContainer_WithNoSolutionMethods_Throws()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<EntryPointNotFoundException>(SUT_SearchSolutionContainer<HasNoSolutionMethods>());
    }

    [Fact]
    public void SearchSolutionContainer_WithMultipleSolutionMethods_Throws()
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

        Assert.Throws<AmbiguousMatchException>(SUT_SearchSolutionContainer<HasMultipleSolutionMethods>());
    }
}



public class HasSingleSolutionMethod
{
    [Solution]
    public int Solution() { return default; }
}

public class HasNoSolutionMethods
{
}

public class HasMultipleSolutionMethods
{
    [Solution]
    public int SolutionOne() { return default; }
    public void SolutionTwo([Result] int result) { }
}