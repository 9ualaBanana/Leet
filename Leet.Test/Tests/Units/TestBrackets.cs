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

using Xunit;
using Lit.Services.StringInterpreter;
using System.Collections.Generic;
using System;

namespace Leet.Test.Tests.Units;

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
