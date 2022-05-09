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
using System;
using System.Reflection;

namespace Leet.Test.Framework.Abstractions.SolutionMethod;

/// <summary>
/// Encapsulates the data required for defining methods inside a dynamic context at runtime.
/// </summary>
public struct SolutionMethodStub
{
    internal MethodAttributes AccessModifier = MethodAttributes.Public;
    internal string Name = Guid.NewGuid().ToString();
    internal bool HasSolutionLabel = false;
    internal Type[]? Parameters = null;
    internal int[]? ResultAttributesPositions = null;
    internal Type? ReturnType = null;

    /// <remarks>
    /// Defined for the use by the system. Use <see cref="NewStub"/> instead.
    /// </remarks>
    public SolutionMethodStub()
    {
    }

    /// <summary>
    /// Provides the builder for this class.
    /// </summary>
    internal static SolutionMethodStubBuilder NewStub => new();

    /// <summary>
    /// Creates the definition for this <see cref="SolutionMethodStub"/> inside <paramref name="contextProvider"/>.
    /// </summary>
    /// <param name="contextProvider">the builder of a runtime context.</param>
    internal void PutInContext(SolutionContextProvider contextProvider)
    {
        contextProvider.DefineMethod(this);
    }
}
