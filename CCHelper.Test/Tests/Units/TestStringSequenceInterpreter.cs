using CCHelper.Services.ArgumentsProcessing.StringInterpreter;
using CCHelper.Test.Framework.TestData;
using System;
using System.Collections.Generic;
using Xunit;

namespace CCHelper.Test.Tests.Units;

public class TestStringSequenceInterpreter
{
    StringSequenceInterpreter SUT_StringSequenceInterpreter(string stringSequence) => new(stringSequence);



    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToEnumerable_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToArray_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToArray());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToJaggedArray_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToEnumerable_UnsupportedString_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToArray_UnsupportedString_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToArray());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Erroneous), MemberType = typeof(StringSequenceData))]
    public void ToJaggedArray_UnsupportedString_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.Empty), MemberType = typeof(StringSequenceData))]
    public void ToEnumerable_EmptyString_Throws(string stringWithBrackets)
    {
        Assert.Throws<InvalidOperationException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Empty), MemberType = typeof(StringSequenceData))]
    public void ToArray_EmptyString_Throws(string stringWithBrackets)
    {
        Assert.Throws<InvalidOperationException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToArray());
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Empty), MemberType = typeof(StringSequenceData))]
    public void ToJaggedArray_EmptyString_Throws(string stringWithBrackets)
    {
        Assert.Throws<InvalidOperationException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(StringSequenceData.NonJagged), MemberType = typeof(StringSequenceData))]
    public void AppropriateInterpreter_StringSequence_ReturnsToArray(string stringSequence, int[] _)
    {
        var sut = SUT_StringSequenceInterpreter(stringSequence);
        var appropriateInterpreter = sut.AppropriateInterpreter;

        Assert.Equal(sut.ToArray, appropriateInterpreter);
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.Jagged), MemberType = typeof(StringSequenceData))]
    public void AppropriateInterpreter_JaggedStringSequence_ReturnsToJaggedArray(string stringSequence, int[][] _)
    {
        var sut = SUT_StringSequenceInterpreter(stringSequence);
        var appropriateInterpreter = sut.AppropriateInterpreter;

        Assert.Equal(sut.ToJaggedArray, appropriateInterpreter);
    }



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
