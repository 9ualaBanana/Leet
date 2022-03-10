using CCHelper.Services.ArgumentsProcessor.StringSequenceInterpreter;
using System;
using System.Collections.Generic;
using Xunit;

namespace CCHelper.Test.Tests.Units;

public class TestStringSequenceInterpreter
{
    static StringSequenceInterpreter SUT_StringSequenceInterpreter(string stringSequence) => new(stringSequence);

    [Theory]
    [InlineData("(>")]
    [InlineData("{]")]
    [InlineData("[)")]
    [InlineData("<}")]
    public void WhenPassedStringWithInconsistentBrackets_ShouldThrow(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_StringSequenceInterpreter(stringWithBrackets).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(StringSequencesAndTheirInterpretation))]
    public void WhenPassedStringSequenceToEnumerable_ShouldReturnInterpretedEnumerable(string stringSequence, int[] interpretedSequence)
    {
        Assert.Equal(SUT_StringSequenceInterpreter(stringSequence).ToEnumerable(), interpretedSequence);
    }

    [Theory]
    [MemberData(nameof(StringSequencesAndTheirInterpretation))]
    public void WhenPassedStringSequenceToArray_ShouldReturnInterpretedArray(string stringSequence, int[] interpretedSequence)
    {
        Assert.Equal(SUT_StringSequenceInterpreter(stringSequence).ToArray(), interpretedSequence);
    }

    [Theory]
    [MemberData(nameof(JaggedStringSequencesAndTheirInterpretation))]
    public void WhenPassedJaggedArrayStringSequence_ShouldReturnInterpretedJaggedArray(string stringSequence, int[][] interpretedSequence)
    {
        Assert.Equal(SUT_StringSequenceInterpreter(stringSequence).ToJaggedArray(), interpretedSequence);
    }
    public static IEnumerable<object[]> JaggedStringSequencesAndTheirInterpretation
    {
        get
        {
            yield return new object[]
            {
                "[[1, 21] [5, 6]]", new int[][] { new int[] { 1, 21 }, new int[] { 5, 6 } }
            };
            yield return new object[]
            {
                "[ [0, -200], [45]]", new int[][] { new int[] { 0, -200 }, new int[] { 45 } }
            };
            yield return new object[]
            {
                "[[10]]", new int[][] { new int[] { 10 } }
            };
            yield return new object[]
            {
                "[[98, 2, 51], [5]]", new int[][] { new int[] { 98, 2, 51 }, new int[] { 5 } }
            };
            yield return new object[]
            {
                "[[11], [12], [32], [44]]", new int[][] { new int[] { 11 }, new int[] { 12 }, new int[] { 32 }, new int[] { 44 } }
            };
            yield return new object[]
            {
                "[[1, 25], [3], [74]]", new int[][] { new int[] { 1, 25 }, new int[] { 3 }, new int[] { 74 } }
            };
        }
    }

    public static IEnumerable<object[]> StringSequencesAndTheirInterpretation
    {
        get
        {
            yield return new object[]
            {
                "[1, 2, 3, 4, 5]", new int[] { 1, 2, 3, 4, 5 }
            };
            yield return new object[]
            {
                "[ +4, -6, -1, +2, +8 ]", new int[] { 4, -6, -1, 2, 8 }
            };
            yield return new object[]
            {
                "[5, 5, +5, 5, 5]", new int[] {5, 5, 5, 5, 5 }
            };
            yield return new object[]
            {
                " [2, 2, 0, -1, -10] ", new int[] {2, 2, 0, -1, -10 }
            };
            yield return new object[]
            {
                " [ 1, 10, 30, -100, 20 ] ", new int[] {1, 10, 30, -100, 20 }
            };
            yield return new object[]
            {
                "[0]", new int[] { 0 }
            };
            yield return new object[]
            {
                "[-1]", new int[] { -1 }
            };
            yield return new object[]
            {
                "[+2]", new int[] { 2 }
            };
            yield return new object[]
            {
                "[ +25]", new int[] { 25 }
            };
            yield return new object[]
            {
                " [ -1000 ]", new int[] { -1000 }
            };
            yield return new object[]
            {
                "  [  -210 ] ", new int[] { -210 }
            };
        }
    }
}
