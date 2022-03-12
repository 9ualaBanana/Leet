using CCHelper.Services.ArgumentsProcessing.ArgumentsFormats;
using CCHelper.Test.Framework.TestData;
using System;
using Xunit;

namespace CCHelper.Test.Tests.Units;

public class TestStringSequenceArgumentsFormat
{
    readonly StringSequenceArgumentsFormat SUT_StringSequenceArgumentsFormat = new();



    [Fact]
    public void Match_Null_ReturnsFalse()
    {
        Assert.False(SUT_StringSequenceArgumentsFormat.Match(null));
    }

    [Fact]
    public void Match_StringAsFirstArgument_ReturnsTrue()
    {
        object?[]? arguments = new object[] { string.Empty };
        Assert.True(SUT_StringSequenceArgumentsFormat.Match(arguments));
    }



    [Fact]
    public void Normalize_CalledPriorToMatch_Throws()
    {
        object?[]? dummy = null;
        Assert.Throws<InvalidOperationException>(() => SUT_StringSequenceArgumentsFormat.Normalize(ref dummy));
    }

    [Theory]
    [MemberData(nameof(StringSequenceData.NonJagged), MemberType = typeof(StringSequenceData))]
    public void Normalize_MatchingArguments_InterpretsAsIntArray(string stringSequence, int[] _)
    {
        object?[]? arguments = new object[] { stringSequence };

        SUT_StringSequenceArgumentsFormat.Match(arguments);
        SUT_StringSequenceArgumentsFormat.Normalize(ref arguments);

        Assert.Equal(arguments![0]!.GetType(), typeof(int[])!);
    }
}
