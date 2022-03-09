using CCHelper.Services;
using CCHelper.Test.Framework.Abstractions.SolutionContext;
using CCHelper.Test.Framework.Abstractions.SolutionMethod;
using CCHelper.Test.Framework.TestData;
using System;
using System.Reflection;
using Xunit;

namespace CCHelper.Test.Tests.Units;

public class TestSolutionMethodValidator : DynamicContextFixture
{
    MethodInfo DefinedSolutionMethod => _context.SolutionContainer.DefinedMethod;
    void SUT_IsValidSolutionMethod() => SolutionMethodValidator.IsValidSolutionMethod(DefinedSolutionMethod);

    [Fact]
    public void WhenSolutionMethodHasMultipleDifferentLabels_ShouldThrow()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SUT_IsValidSolutionMethod);
    }

    [Fact]
    public void WhenOutputSolutionReturnTypeIsVoid_ShouldThrow()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<FormatException>(SUT_IsValidSolutionMethod);
    }

    [Fact]
    public void WhenInputSolutionHasMultipleResultLabels_ShouldThrow()
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType, TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1, 2)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SUT_IsValidSolutionMethod);
    }

    [Theory]
    [MemberData(nameof(TypeData.Types), MemberType = typeof(TypeData))]
    public void WhenInputSolutionReturnTypeIsNotVoid_ShouldThrow(Type returnType)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(returnType)
            .PutInContext(_context);

        Assert.Throws<FormatException>(SUT_IsValidSolutionMethod);
    }

    [Fact]
    public void WhenResultLabelApplied_ShouldHaveResultLabel()
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.True(SolutionMethodValidator.HasResultLabel(DefinedSolutionMethod));
    }

    [Fact]
    public void WhenResultLabelIsNotApplied_ShouldNotHaveResultLabel()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.False(SolutionMethodValidator.HasResultLabel(DefinedSolutionMethod));
    }

    [Fact]
    public void WhenSolutionLabelApplied_ShouldHaveSolutionLabel()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.True(SolutionMethodValidator.HasSolutionLabel(DefinedSolutionMethod));
    }

    [Fact]
    public void WhenSolutionLabelIsNotApplied_ShouldNotHaveSolutionLabel()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.False(SolutionMethodValidator.HasResultLabel(DefinedSolutionMethod));
    }
}
