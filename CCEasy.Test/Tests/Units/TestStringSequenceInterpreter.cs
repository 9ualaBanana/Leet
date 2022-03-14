using CCEasy.Services.ArgumentsProcessing.StringInterpreter;
using CCEasy.Test.Framework.TestData;
using System;
using Xunit;

namespace CCEasy.Test.Tests.Units;

public class TestStringSequenceInterpreter
{
    StringSequenceInterpreter<TInterpreted> SUT_StringSequenceInterpreter<TInterpreted>(
        string stringSequence,
        Func<string, TInterpreted> interpreter
        )
    {
        return new(stringSequence, interpreter);
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToEnumerable_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets, int.Parse).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToArray_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets, int.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToJaggedArray_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets, int.Parse).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToEnumerable_UnsupportedString_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets, int.Parse).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToArray_UnsupportedString_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets, int.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToJaggedArray_UnsupportedString_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets, int.Parse).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.Empty), MemberType = typeof(StringSequenceData))]
    public void ToEnumerable_EmptyString_Throws(string stringWithBrackets)
    {
        Assert.Throws<InvalidOperationException>(() => SUT_StringSequenceInterpreter(stringWithBrackets, int.Parse).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Empty), MemberType = typeof(StringSequenceData))]
    public void ToArray_EmptyString_Throws(string stringWithBrackets)
    {
        Assert.Throws<InvalidOperationException>(() => SUT_StringSequenceInterpreter(stringWithBrackets, int.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Empty), MemberType = typeof(StringSequenceData))]
    public void ToJaggedArray_EmptyString_Throws(string stringWithBrackets)
    {
        Assert.Throws<InvalidOperationException>(() => SUT_StringSequenceInterpreter(stringWithBrackets, int.Parse).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.NonJagged), MemberType = typeof(StringSequenceData))]
    public void AppropriateInterpreter_StringSequence_ReturnsToArray(string stringSequence, int[] _)
    {
        var sut = SUT_StringSequenceInterpreter(stringSequence, int.Parse);
        var appropriateInterpreter = sut.AppropriateInterpreter;

        Assert.Equal(sut.ToArray, appropriateInterpreter);
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Jagged), MemberType = typeof(StringSequenceData))]
    public void AppropriateInterpreter_JaggedStringSequence_ReturnsToJaggedArray(string stringSequence, int[][] _)
    {
        var sut = SUT_StringSequenceInterpreter(stringSequence, int.Parse);
        var appropriateInterpreter = sut.AppropriateInterpreter;

        Assert.Equal(sut.ToJaggedArray, appropriateInterpreter);
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.NonJagged), MemberType = typeof(StringSequenceData))]
    public void ToEnumerable_StringSequence_ReturnsInterpretedSequence(string stringSequence, int[] interpretedSequence)
    {
        Assert.Equal(interpretedSequence, SUT_StringSequenceInterpreter(stringSequence, int.Parse).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.NonJagged), MemberType = typeof(StringSequenceData))]
    public void ToArray_StringSequence_ReturnsInterpretedSequence(string stringSequence, int[] interpretedSequence)
    {
        Assert.Equal(interpretedSequence, SUT_StringSequenceInterpreter(stringSequence, int.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Jagged), MemberType = typeof(StringSequenceData))]
    public void ToJaggedArray_StringSequence_ReturnsInterpretedSequence(string stringSequence, int[][] interpretedSequence)
    {
        Assert.Equal(interpretedSequence, SUT_StringSequenceInterpreter(stringSequence, int.Parse).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.NonJaggedDouble), MemberType = typeof(StringSequenceData))]
    public void ToArray_StringSequenceOfDoubles_ReturnsInterpretedSequence(string stringSequence, double[] interpretedSequence)
    {
        Assert.Equal(interpretedSequence, SUT_StringSequenceInterpreter(stringSequence, double.Parse).ToArray());
    }
}
