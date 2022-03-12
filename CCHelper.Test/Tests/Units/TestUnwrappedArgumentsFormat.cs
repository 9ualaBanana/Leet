using CCHelper.Services.ArgumentsProcessing.ArgumentsFormats;
using System;
using Xunit;

namespace CCHelper.Test.Tests.Units;

public class TestUnwrappedArgumentsFormat
{
    readonly UnwrappedArgumentsFormat SUT_UnwrappedArgumentsFormat = new();



    [Fact]
    public void Match_Null_ReturnsTrue()
    {
        Assert.True(SUT_UnwrappedArgumentsFormat.Match(null));
    }

    [Fact]
    public void Match_NotObjectArray_ReturnsTrue()
    {
        object?[]? arguments = new int[][] { new int[] { default }, new int[] { default } };
        Assert.True(SUT_UnwrappedArgumentsFormat.Match(arguments));
    }



    [Fact]
    public void Normalize_CalledPriorToMatch_Throws()
    {
        object?[]? dummy = null;
        Assert.Throws<InvalidOperationException>(() => SUT_UnwrappedArgumentsFormat.Normalize(ref dummy));
    }

    [Fact]
    public void Normalize_MatchingArguments_WrapsInObjectArray()
    {
        object?[]? arguments = new int[][] { new int[] { default }, new int[] { default } };
        var wrappedArguments = new object[] { arguments };

        SUT_UnwrappedArgumentsFormat.Match(arguments);
        SUT_UnwrappedArgumentsFormat.Normalize(ref arguments);

        Assert.Equal(wrappedArguments, arguments);
    }
}
