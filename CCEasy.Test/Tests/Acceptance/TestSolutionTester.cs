using CCEasy.Test.Framework.Abstractions;
using CCEasy.Test.Framework.Abstractions.SolutionContext;
using CCEasy.Test.Framework.Abstractions.SolutionMethod;
using CCEasy.Test.Framework.TestData;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace CCEasy.Test.Tests.Acceptance;

public class TestSolutionTester : DynamicContextFixture
{
    Action SUT_SolutionTesterConstructor<TResult>(TResult _)
    {
        static void callConstructor<TSolutionContainer>(TSolutionContainer solutionContainer)
            where TSolutionContainer : class, new() =>
            new SolutionTester<TSolutionContainer, TResult>(solutionContainer);
        // Must call the constructor with the dynamically pre-instantiated SolutionContainer.Instance.
        // The default constructor instantiates a new TSolutionContainer object statically which won't work.

        return () => callConstructor(_context.SolutionContainer.Instance);
    }

    [Theory]
    [MemberData(nameof(ValidSolutionMethods))]
    public void WhenSolutionContainerDefinesValidSolutionMethod_ShouldNotThrow(SolutionMethodStub solutionMethodStub)
    {
        solutionMethodStub.PutInContext(_context);

        Assert.Null(Record.Exception(SUT_SolutionTesterConstructor(TypeData.DummyValue)));
    }

    [Theory]
    [InlineData(AccessModifier.Internal)]
    [InlineData(AccessModifier.Protected)]
    [InlineData(AccessModifier.Private)]
    public void WhenNoSolutionMethodsWereDiscovered_ShouldThrow(AccessModifier accessModifier)
    {
        SolutionMethodStub
            .NewStub
            .WithAccessModifier(accessModifier)
            .WithSolutionLabel
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<EntryPointNotFoundException>(SUT_SolutionTesterConstructor(TypeData.DummyType));
    }

    [Fact]
    public void WhenMultipleSolutionMethodsWereDiscovered_ShouldThrow()
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

        Assert.Throws<AmbiguousMatchException>(SUT_SolutionTesterConstructor(TypeData.DummyType));
    }

    [Fact]
    public void WhenBothAttributesApplied_ShouldThrow()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SUT_SolutionTesterConstructor(TypeData.DummyType));
    }

    [Fact]
    public void WhenMultipleResultAttributesApplied_ShouldThrow()
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType, TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1, 2)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SUT_SolutionTesterConstructor(TypeData.DummyType));
    }

    [Fact]
    public void WhenOutputSolutionReturnsVoid_ShouldThrow()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<FormatException>(SUT_SolutionTesterConstructor(TypeData.DummyType));
    }

    [Theory]
    [MemberData(nameof(TypeData.ValueTypes), MemberType = typeof(TypeData))]
    public void WhenInputSolutionDoesNotReturnVoid_ShouldThrow(Type nonVoidType)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(nonVoidType)
            .PutInContext(_context);

        Assert.Throws<FormatException>(SUT_SolutionTesterConstructor(TypeData.DummyValue));
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
