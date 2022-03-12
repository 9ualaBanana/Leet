using CCHelper.Services.ArgumentsProcessor.StringInterpreter;
using CCHelper.Test.Framework.TestData;
using System;
using System.Collections.Generic;
using Xunit;

namespace CCHelper.Test.Tests.Units;

public class TestStringSequenceInterpreter
{
    static StringSequenceInterpreter SUT_StringSequenceInterpreter(string stringSequence) => new(stringSequence);



    [Theory]
    [MemberData(nameof(InconsistentBrackets))]
    public void ToEnumerable_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(InconsistentBrackets))]
    public void ToArray_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToArray());
    }

    [Theory]
    [MemberData(nameof(InconsistentBrackets))]
    public void ToJaggedArray_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToJaggedArray());
    }

    public static IEnumerable<object[]> InconsistentBrackets
    {
        get
        {
            yield return new object[] { "(>" };
            yield return new object[] { "{]" };
            yield return new object[] { "[)" };
            yield return new object[] { "<}" };
        }
    }

    public static IEnumerable<object[]> StringSequencesAndTheirInterpretation
    {

    [Theory]
    [MemberData(nameof(StringSequenceData.NonJagged), MemberType = typeof(StringSequenceData))]
    public void ToEnumerable_StringSequence_ReturnsInterpretedSequence(string stringSequence, int[] interpretedSequence)
    {
        Assert.Equal(interpretedSequence, SUT_StringSequenceInterpreter(stringSequence).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.NonJagged), MemberType = typeof(StringSequenceData))]
    public void ToArray_StringSequence_ReturnsInterpretedSequence(string stringSequence, int[] interpretedSequence)
    {
        Assert.Equal(interpretedSequence, SUT_StringSequenceInterpreter(stringSequence).ToArray());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Jagged), MemberType = typeof(StringSequenceData))]
    public void ToJaggedArray_StringSequence_ReturnsInterpretedSequence(string stringSequence, int[][] interpretedSequence)
    {
        Assert.Equal(interpretedSequence, SUT_StringSequenceInterpreter(stringSequence).ToJaggedArray());
    }
}
