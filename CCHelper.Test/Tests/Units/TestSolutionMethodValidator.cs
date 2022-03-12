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
    public void SUT_SolutionMethodHasMultipleDifferentLabels_Throws()
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
    public void SUT_SolutionMethodHasMultipleResultLabels_Throws()
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType, TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1, 2)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SUT_IsValidSolutionMethod);
    }



    [Fact]
    public void HasSolutionLabel_SolutionLabelApplied_ReturnsTrue()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.True(SolutionMethodValidator.HasSolutionLabel(DefinedSolutionMethod));
    }

    [Fact]
    public void HasSolutionLabel_SolutionLableNotApplied_ReturnsFalse()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.False(SolutionMethodValidator.HasResultLabel(DefinedSolutionMethod));
    }


    [Fact]
    public void HasResultLabel_ResultLabelApplied_ReturnsTrue()
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
    public void HasResultLabel_ResultLabelNotApplied_ReturnsFalse()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.False(SolutionMethodValidator.HasResultLabel(DefinedSolutionMethod));
    }



    [Fact]
    public void SUT_OutputSolutionReturnTypeIsVoid_Throws()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<FormatException>(SUT_IsValidSolutionMethod);
    }

    [Theory]
    [MemberData(nameof(TypeData.ValueTypes), MemberType = typeof(TypeData))]
    [MemberData(nameof(TypeData.NullableTypes), MemberType = typeof(TypeData))]
    [MemberData(nameof(TypeData.ReferenceTypes), MemberType = typeof(TypeData))]
    public void SUT_InputSolutionReturnTypeIsNotVoid_Throws(Type returnType)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(returnType)
            .PutInContext(_context);

        Assert.Throws<FormatException>(SUT_IsValidSolutionMethod);
    }
}
