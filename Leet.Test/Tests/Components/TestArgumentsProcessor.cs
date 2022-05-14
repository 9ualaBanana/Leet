//MIT License

//Copyright (c) 2022 GualaBanana

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using Leet.Services;
using Leet.Test.Framework.Abstractions.SolutionContext;
using Leet.Test.Framework.Abstractions.SolutionMethod;
using Leet.Test.Framework.TestData;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Leet.Test.Tests.Components;

public class TestArgumentsProcessor : DynamicContextFixture
{
    Func<object?[]?> SUT_ProcessArguments(params object?[]? arguments)
    {

        return () => new ArgumentsProcessor<int>(_context.SolutionContainer.DefinedMethod, arguments, int.Parse).Process();
    }

    Func<object?[]?> SUT_ProcessArguments<TInterpreted>(Func<string, TInterpreted> interpreter, params object?[]? arguments)
    {
        return () => new ArgumentsProcessor<TInterpreted>(_context.SolutionContainer.DefinedMethod, arguments, interpreter).Process();
    }



    [Fact]
    public void SolutionMethodWithNoParameter_EmptyArguments_DoesNotThrow()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(EmptyArgumentsList)));
    }

    [Fact]
    public void InputSolution_EmptyArguments_Throws()
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
    public void SolutionMethod_WrongNumberOfArguments_Throws(int parametersCount, int argumentsCount)
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
    public void SolutionMethod_WrongTypeArguments_Throws(object argument)
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
    public void SolutionMethodWithNullableParameters_NullArguments_DoesNotThrow(Type[] parametersTypes)
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
    public void SolutionMethodWithReferenceParameters_NullArguments_DoesNotThrow(Type[] parametersTypes)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(parametersTypes)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(GetNullArguments(parametersTypes.Length))));
    }


    [Fact]
    public void WhenParamsUnwrapsArrayArguments_DoesNotThrow()
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
    public void SolutionMethod_SeparatelyPassedArguments_DoesNotThrow(object value)
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
    public void SolutionMethod_SingleNull_DoesNotThrow(Type canHoldNull)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(canHoldNull)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(null)));
    }



    [Theory]
    [MemberData(nameof(CollectionInStringData.NonJagged), MemberType = typeof(CollectionInStringData))]
    public void SolutionMethod_NonJaggedStringSequence_DoesNotThrow(string stringSequence, int[] _)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(int[]))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(stringSequence)));
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Jagged), MemberType = typeof(CollectionInStringData))]
    public void SolutionMethod_JaggedStringSequence_DoesNotThrow(string stringSequence, int[][] _)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(int[][]))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_ProcessArguments(stringSequence)));
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Erroneous), MemberType = typeof(CollectionInStringData))]
    public void SolutionMethodWithArrayParameter_NonSequenceString_Throws(string unsupportedString)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.NotNull(Record.Exception(SUT_ProcessArguments(unsupportedString)));
    }



    [Theory]
    [MemberData(nameof(CollectionInStringData.NonJagged), MemberType = typeof(CollectionInStringData))]
    public void SolutionMethod_StringSequenceWithOtherArguments_CorrectlyProcesses(
        string stringSequence, int[] interpretedSequence
        )
    {
        object?[]? arguments = new object[] { stringSequence, TypeData.DummyValue };
        object?[]? expectedProcessedArguments = new object[] { interpretedSequence, TypeData.DummyValue };
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(int[]), TypeData.DummyType)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        var actualProcessedArguments = SUT_ProcessArguments(arguments).Invoke();

        Assert.Equal(expectedProcessedArguments, actualProcessedArguments);
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Jagged), MemberType = typeof(CollectionInStringData))]
    [MemberData(nameof(CollectionInStringData.NonJagged), MemberType = typeof(CollectionInStringData))]
    public void SolutionMethodWithStringParameter_StringSequence_DoesNotInterpret(string stringSequence, object _)
    {
        object?[]? arguments = new object[] { stringSequence };
        object?[]? expectedProcessedArguments = new object[] { stringSequence };
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(string))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        var actualProcessedArguments = SUT_ProcessArguments(arguments).Invoke();

        Assert.Equal(expectedProcessedArguments, actualProcessedArguments);
    }

    [Fact]
    public void SolutionMethodWithArrayParameters_MultipleStringSequences_CorrectlyInterprets()
    {
        object?[]? arguments = new object[] { "[1, 2, 3]", "{ .3, 1.5, -69 }" };
        object?[]? expectedProcessedArguments = new object[] { new double[] { 1, 2, 3 }, new double[] { .3, 1.5, -69 } };
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(double[]), typeof(double[]))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        var actualProcessedArguments = SUT_ProcessArguments(double.Parse, arguments).Invoke();

        Assert.Equal(expectedProcessedArguments, actualProcessedArguments);
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.NonJaggedDouble), MemberType = typeof(CollectionInStringData))]
    public void SolutionMethod_StringSequenceWithDoubleParameters_CorrectlyInterprets(string stringSequence, double[] interpretedSequence)
    {
        object?[]? arguments = new object[] { stringSequence };
        object?[]? expectedProcessedArguments = new object[] { interpretedSequence };
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(double[]))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        var actualProcessedArguments = SUT_ProcessArguments(double.Parse, arguments).Invoke();

        Assert.Equal(expectedProcessedArguments, actualProcessedArguments);
    }

    [Fact]
    public void SolutionMethod_StringSequencesWithDifferentRanks_CorrectlyInterprets()
    {
        object?[]? arguments = new object[] { "[1, 2, 3]", "{ { .3, 1.5, -69 }, { -1, +.5 } }" };
        object?[]? expectedProcessedArguments = new object[] {
            new double[] { 1, 2, 3 },
            new double[][] { new double[] {.3, 1.5, -69 }, new double[] { -1, .5 } }
        };
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(double[]), typeof(double[][]))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        var actualProcessedArguments = SUT_ProcessArguments(double.Parse, arguments).Invoke();

        Assert.Equal(expectedProcessedArguments, actualProcessedArguments);
    }

    [Fact]
    public void SolutionMethod_StringSequencesWithDifferentTypeElements_Throws()
    {
        object?[]? arguments = new object[] { "[1, 2, 3]", "{ { .3, 1.5, -69 }, { -1, +.5 } }" };
        object?[]? expectedProcessedArguments = new object[] {
            new int[] { 1, 2, 3 },
            new double[][] { new double[] {.3, 1.5, -69 }, new double[] { -1, .5 } }
        };
        SolutionMethodStub
            .NewStub
            .Accepting(typeof(int[]), typeof(double[][]))
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<ArgumentException>(SUT_ProcessArguments(int.Parse, arguments));
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