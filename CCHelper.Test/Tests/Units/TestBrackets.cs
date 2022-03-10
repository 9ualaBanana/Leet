using Xunit;
using CCHelper.Services.ArgumentsProcessor.StringSequenceInterpreter;
using System.Collections.Generic;
using System;

namespace CCHelper.Test.Tests.Units;

public class TestBrackets
{
    [Theory]
    [MemberData(nameof(BracketsTypes))]
    internal void WhenBracketsAreSupported_ShouldReturnTrue(BracketsType bracketsType)
    {
        var brackets = new Brackets(bracketsType);

        Assert.True(brackets.AreSupported);
    }

    public static IEnumerable<object[]> BracketsTypes
    {
        get
        {
            foreach (var bracketsType in Enum.GetValues(typeof(BracketsType)))
            {
                yield return new object[] { (BracketsType)bracketsType };
            }
        }
    }
}
