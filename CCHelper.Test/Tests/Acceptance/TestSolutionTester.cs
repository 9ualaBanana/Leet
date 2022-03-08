using CCHelper.Test.Framework;
using CCHelper.Test.Framework.Abstractions;
using CCHelper.Test.Framework.Abstractions.SolutionContext;
using CCHelper.Test.Framework.Abstractions.SolutionMethod;
using CCHelper.Test.Framework.TestData;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace CCHelper.Test.Tests.Acceptance;

public class TestSolutionTester : DynamicContextClient
{
    Action SolutionTesterConstructor(Type tResult)
    {
        var solutionTesterType = typeof(SolutionTester<,>).MakeGenericType(_context.SolutionContainer.Type, tResult);
        var constructor = solutionTesterType.GetConstructor(Type.EmptyTypes)!;
        return () => constructor.Invoke(BindingFlags.DoNotWrapExceptions, null, null, null);
    }

    [Theory]
    [MemberData(nameof(ValidSolutionMethods))]
    public void ShouldNotThrow_WhenSolutionContainerDefinesValidSolutionMethod(SolutionMethodStub solutionMethodStub)
    {
        solutionMethodStub.PutInContext(_context);

        Assert.True(SolutionTesterConstructor(TypeData.DummyType).DoesNotThrow());
    }

    [Theory]
    [InlineData(AccessModifier.Internal)]
    [InlineData(AccessModifier.Protected)]
    [InlineData(AccessModifier.Private)]
    public void ShouldThrowEntryPointNotFoundException_WhenNoSolutionMethodsWereDiscovered(AccessModifier accessModifier)
    {
        SolutionMethodStub
            .NewStub
            .WithAccessModifier(accessModifier)
            .WithSolutionLabel
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<EntryPointNotFoundException>(SolutionTesterConstructor(TypeData.DummyType));
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenMultipleSolutionMethodsWereDiscovered()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .WithSolutionLabel
            .PutInContext(_context);
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SolutionTesterConstructor(TypeData.DummyType));
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenBothAttributesApplied()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SolutionTesterConstructor(TypeData.DummyType));
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenMultipleResultAttributesApplied()
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType, TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1, 2)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SolutionTesterConstructor(TypeData.DummyType));
    }

    [Fact]
    public void ShouldThrowFormatException_WhenOutputSolutionReturnsVoid()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<FormatException>(SolutionTesterConstructor(TypeData.DummyType));
    }

    [Theory]
    [MemberData(nameof(TypeData.Types), MemberType = typeof(TypeData))]
    public void ShouldThrowFormatException_WhenInputSolutionDoesNotReturnVoid(Type nonVoidType)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .Returning(nonVoidType)
            .WithResultLabelAppliedToParameter(1)
            .PutInContext(_context);

        Assert.Throws<FormatException>(SolutionTesterConstructor(TypeData.DummyType));
    }

    public static IEnumerable<object[]> ValidSolutionMethods
    {
        get
        {
            yield return new object[] {
            SolutionMethodStub
                .NewStub
                .WithSolutionLabel
                .Returning(TypeData.DummyType)
                .Build()
            };

            yield return new object[] {
            SolutionMethodStub
                .NewStub
                .Accepting(TypeData.DummyType)
                .WithResultLabelAppliedToParameter(1)
                .Returning(typeof(void))
                .Build()
            };
        }
    }
}
