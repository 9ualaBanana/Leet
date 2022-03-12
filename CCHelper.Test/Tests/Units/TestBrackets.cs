using Xunit;
using CCHelper.Services.ArgumentsProcessor.StringInterpreter;
using System.Collections.Generic;
using System;

namespace CCHelper.Test.Tests.Units;

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
