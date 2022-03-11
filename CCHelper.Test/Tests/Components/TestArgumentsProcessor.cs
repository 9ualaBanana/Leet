using CCHelper.Services.ArgumentsProcessor;
using CCHelper.Test.Framework.Abstractions.SolutionContext;
using CCHelper.Test.Framework.Abstractions.SolutionMethod;
using CCHelper.Test.Framework.TestData;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace CCHelper.Test.Tests.Components;

public class TestArgumentsProcessor : DynamicContextFixture
{
    Action SUT_ProcessArguments(params object?[]? arguments)
    {
        return () => new ArgumentsProcessor(_context.SolutionContainer.DefinedMethod, arguments).Process();
    }

    [Theory]
    [MemberData(nameof(TypeData.DefaultValues), MemberType = typeof(TypeData))]
    public void WhenArgumentsOfWrongTypePassed_ShouldThrow(object argument)
    {
        Type parameterType = typeof(int);
        if (argument.GetType() == parameterType) return;

        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(parameterType)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<ArgumentException>(SUT_ProcessArguments(argument));
    }

    [Theory]
    [InlineData(new object[] { new Type[] { typeof(int?) } })]
    [InlineData(new object[] { new Type[] { typeof(bool?), typeof(double?) } })]
    public void WhenCorrespondingParametersAreNullableTypes_ShouldAcceptNullArguments(Type[] parametersTypes)
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(parametersTypes)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.DoesNotThrow(SUT_ProcessArguments(GetNullArguments(parametersTypes.Length)));
    }

    [Theory]
    [InlineData(new object[] { new Type[] { typeof(string) } })]
    [InlineData(new object[] { new Type[] { typeof(string), typeof(object) } })]
    public void WhenCorrespondingParametersAreReferenceTypes_ShouldAcceptNullArguments(Type[] parametersTypes)
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(parametersTypes)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.DoesNotThrow(SUT_ProcessArguments(GetNullArguments(parametersTypes.Length)));
    }

    [Fact]
    public void WhenSolutionMethodHasNoParameters_ShouldAcceptEmptyArguments()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.DoesNotThrow(SUT_ProcessArguments(EmptyArgumentsList));
    }

    [Theory]
    [MemberData(nameof(TypeData.DefaultValues), MemberType = typeof(TypeData))]
    public void WhenArgumentsPassedAsSeparateElements_ShouldWrapThem(object value)
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(value.GetType(), value.GetType())
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.DoesNotThrow(SUT_ProcessArguments(value, value));
    }

    [Fact]
    public void WhenSingleNullArgumentPassed_ShouldWrapIt()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(typeof(object))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.DoesNotThrow(SUT_ProcessArguments(null));
    }

    [Theory]
    [InlineData(new object[] { new int[] { default } })]
    [InlineData(new object[] { new int[] { default, default, default } })]
    public void WhenParamsUnwrapsArrayArguments_ShouldWrapThem(object arguments)
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(arguments.GetType())
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.DoesNotThrow(SUT_ProcessArguments(arguments));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(1, 2)]
    [InlineData(4, 3)]
    public void WhenWrongNumberOfArgumentsPassed_ShouldThrow(int parametersCount, int argumentsCount)
    {
        var dummyParameters = new Type[parametersCount];
        Array.Fill(dummyParameters, TypeData.DummyType);
        var dummyArguments = new object[argumentsCount];
        Array.Fill(dummyArguments, new object());
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(dummyParameters)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<TargetParameterCountException>(SUT_ProcessArguments(dummyArguments));
    }

    [Fact]
    public void WhenNoArgumentsPassedToInputSolution_ShouldThrow()
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<TargetParameterCountException>(SUT_ProcessArguments(EmptyArgumentsList));
    }

    [Theory(Skip = "Obsolete. null arguments used to be not allowed.")]
    [InlineData(new object[] { null! })]
    [InlineData(new object[] { new object[] { null!, null! } })]
    [InlineData(new object[] { new object[] { default(int), null! } })]
    public void WhenNullArgumentsPassed_ShouldThrow(object[] arguments)
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(TypeData.DummyType, TypeData.DummyType)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<ArgumentNullException>(SUT_ProcessArguments(arguments));
    }

    /// <summary>
    /// Imitates passing no arguments to the public interface that takes arguments in form of `params object[]`
    /// </summary>
    static object[] EmptyArgumentsList => Array.Empty<object>();

    static object?[] GetNullArguments(int length)
    {
        var nullArguments = new List<object?>(length);
        for (int i = 0; i < length; i++) nullArguments.Add(null);
        return nullArguments.ToArray();
    }
}