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

using Leet.Test.Framework.Abstractions.SolutionContext;
using Leet.Test.Framework.Abstractions.SolutionMethod;
using Leet.Test.Framework.TestData;
using System;
using System.Reflection;
using Xunit;

namespace Leet.Test.Tests.Units;

public class TestSolutionMethodValidator : DynamicContextFixture
{
    MethodInfo DefinedSolutionMethod => _context.SolutionContainer.DefinedMethod;
    void SUT_IsValidSolutionMethod() => SolutionMethod.IsValid(DefinedSolutionMethod);



    [Fact]
    public void SUT_SolutionMethodHasMultipleDifferentLabels_Throws()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SUT_IsValidSolutionMethod);
    }

    [Fact]
    public void SUT_SolutionMethodHasMultipleResultLabels_Throws()
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType, TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1, 2)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SUT_IsValidSolutionMethod);
    }

    [Fact]
    public void SUT_OutputSolutionReturnTypeIsVoid_Throws()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<FormatException>(SUT_IsValidSolutionMethod);
    }

    [Theory]
    [MemberData(nameof(TypeData.ValueTypes), MemberType = typeof(TypeData))]
    [MemberData(nameof(TypeData.NullableTypes), MemberType = typeof(TypeData))]
    [MemberData(nameof(TypeData.ReferenceTypes), MemberType = typeof(TypeData))]
    public void SUT_InputSolutionReturnTypeIsNotVoid_Throws(Type returnType)
    {
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(returnType)
            .PutInContext(_context);

        Assert.Throws<FormatException>(SUT_IsValidSolutionMethod);
    }
}
