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

using System.Reflection;

namespace CCEasy.Services;

internal static class SolutionMethodDiscoverer
{
    internal static MethodInfo SearchSolutionContainer<TSolutionContainer>()
    {
        return DiscoverSolutionMethod<TSolutionContainer>();
    }
    static MethodInfo DiscoverSolutionMethod<TSolutionContainer>()
    {
        return GetSingleSolutionInContainerOrThrow<TSolutionContainer>();
    }
    static MethodInfo GetSingleSolutionInContainerOrThrow<TSolutionContainer>()
    {
        var validSolutionMethods = FindValidSolutionMethods<TSolutionContainer>();

        if (!validSolutionMethods.Any())
        {
            throw new EntryPointNotFoundException("Solution method was not found inside the provided solution container.");
        }
        if (validSolutionMethods.Count() > 1)
        {
            throw new AmbiguousMatchException("Solution container must contain exactly one solution method.");
        }

        return validSolutionMethods.Single();
    }
    static IEnumerable<MethodInfo> FindValidSolutionMethods<TSolutionContainer>()
    {
        return typeof(TSolutionContainer).GetMethods().Where(SolutionMethodValidator.IsValidSolutionMethod);
    }
}