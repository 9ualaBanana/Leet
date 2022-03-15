using Xunit;
using CCEasy.Services.StringInterpreter;
using System.Collections.Generic;
using System;

namespace CCEasy.Test.Tests.Units;

public class TestBrackets
{
    [Theory]
    [MemberData(nameof(BracketsTypes))]
    internal void AreSupported_SupportedBrackets_ReturnsTrue(BracketsType bracketsType)
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
