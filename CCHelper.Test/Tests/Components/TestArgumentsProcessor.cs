using CCHelper.Services.ArgumentsProcessing;
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



    [Fact]
    public void WhenEmptyArgumentsPassedToSolutionMethodWithNoParameters_ShouldNotThrow()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(EmptyArgumentsList)));
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
            .Accepting(dummyParameters)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<TargetParameterCountException>(SUT_ProcessArguments(dummyArguments));
    }



    [Theory]
    [MemberData(nameof(TypeData.DefaultValues), MemberType = typeof(TypeData))]
    public void WhenArgumentsOfWrongTypePassed_ShouldThrow(object argument)
    {
        Type parameterType = typeof(int);
        if (argument.GetType() == parameterType) return;

        SolutionMethodStub
            .NewStub
            .Accepting(parameterType)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<ArgumentException>(SUT_ProcessArguments(argument));
    }

    [Theory]
    [InlineData(new object[] { new Type[] { typeof(int?) } })]
    [InlineData(new object[] { new Type[] { typeof(bool?), typeof(double?) } })]
    public void WhenCorrespondingParametersAreNullableTypes_ShouldNotThrow(Type[] parametersTypes)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(parametersTypes)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(GetNullArguments(parametersTypes.Length))));
    }

    [Theory]
    [InlineData(new object[] { new Type[] { typeof(string) } })]
    [InlineData(new object[] { new Type[] { typeof(string), typeof(object) } })]
    public void WhenCorrespondingParametersAreReferenceTypes_ShouldNotThrow(Type[] parametersTypes)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(parametersTypes)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(GetNullArguments(parametersTypes.Length))));
    }


    [Fact]
    public void WhenParamsUnwrapsArrayArguments_ShouldNotThrow()
    {
        var arguments = new int[][] { new int[] { default }, new int[] { default } };
        SolutionMethodStub
            .NewStub
            .Accepting(arguments.GetType())
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(arguments)));
    }

    [Theory]
    [MemberData(nameof(TypeData.DefaultValues), MemberType = typeof(TypeData))]
    public void WhenArgumentsPassedAsSeparateElements_ShouldNotThrow(object value)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(value.GetType(), value.GetType())
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(value, value)));
    }

    [Theory]
    [MemberData(nameof(TypeData.ReferenceTypes), MemberType = typeof(TypeData))]
    [MemberData(nameof(TypeData.NullableTypes), MemberType = typeof(TypeData))]
    public void WhenSingleNullArgumentPassed_ShouldNotThrow(Type canHoldNull)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(canHoldNull)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(null)));
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.NonJagged), MemberType = typeof(StringSequenceData))]
    public void WhenNonJaggedStringSequenceArgumentPassed_ShouldNotThrow(string stringSequence, int[] _)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(int[]))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(stringSequence)));
    }
    
    [Theory]
    [MemberData(nameof(StringSequenceData.Jagged), MemberType = typeof(StringSequenceData))]
    public void WhenJaggedStringSequenceArgumentPassed_ShouldNotThrow(string stringSequence, int[][] _)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(int[][]))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(stringSequence)));
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void WhenUnsupportedStringArgumentPassed_ShouldThrow(string unsupportedString)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.NotNull(Record.Exception(SUT_ProcessArguments(unsupportedString)));
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