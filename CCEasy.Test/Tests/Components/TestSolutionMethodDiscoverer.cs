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

using CCEasy.Services;
using CCEasy.Test.Framework.Abstractions.SolutionContext;
using CCEasy.Test.Framework.Abstractions.SolutionMethod;
using CCEasy.Test.Framework.TestData;
using System;
using System.Reflection;
using Xunit;

namespace CCEasy.Test.Tests.Components;

public class TestSolutionMethodDiscoverer : DynamicContextFixture
{
    Action SUT_SearchSolutionContainer<TSolutionContainer>()
    {
        return () => SolutionMethodDiscoverer.SearchSolutionContainer<TSolutionContainer>();
    }



    [Fact]
    public void SearchSolutionContainer_WithSingleSolutionMethod_DoesNotThrow()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Null(Record.Exception(SUT_SearchSolutionContainer<HasSingleSolutionMethod>));
    }

    [Fact]
    public void SearchSolutionContainer_WithNoSolutionMethods_Throws()
    {
        SolutionMethodStub
            .NewStub
            .Returning(TypeData.DummyType)
            .PutInContext(_context);

        Assert.Throws<EntryPointNotFoundException>(SUT_SearchSolutionContainer<HasNoSolutionMethods>());
    }

    [Fact]
    public void SearchSolutionContainer_WithMultipleSolutionMethods_Throws()
    {
        SolutionMethodStub
            .NewStub
            .WithSolutionLabel
            .Returning(TypeData.DummyType)
            .PutInContext(_context);
        SolutionMethodStub
            .NewStub
            .Accepting(TypeData.DummyType)
            .WithResultLabelAppliedToParameter(1)
            .Returning(typeof(void))
            .PutInContext(_context);

        Assert.Throws<AmbiguousMatchException>(SUT_SearchSolutionContainer<HasMultipleSolutionMethods>());
    }
}



public class HasSingleSolutionMethod
{
    [Solution]
    public int Solution() { return default; }
}

public class HasNoSolutionMethods
{
}

public class HasMultipleSolutionMethods
{
    [Solution]
    public int SolutionOne() { return default; }
    public void SolutionTwo([Result] int result) { }
}