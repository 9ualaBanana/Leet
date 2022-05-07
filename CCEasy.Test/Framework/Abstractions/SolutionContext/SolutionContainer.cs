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

using System;
using System.Reflection;

namespace CCEasy.Test.Framework.Abstractions.SolutionContext;

// Must be public to be seen by Reflection.
/// <summary>
/// Runtime type defined inside <see cref="SolutionContextProvider"/> that contains definitions for <see cref="SolutionMethodStub"/>.
/// </summary>
public class SolutionContainer
{
    internal Type Type { get; }
    internal object Instance => Type.GetConstructor(Type.EmptyTypes)!.Invoke(null);
    // Collection of MethodInfo apparently has to be obtained via Type.GetMethods()
    // because if MethodInfo is accessed in different way NotImplementedException is thrown
    // because of something related to a dynamic module.
    internal MethodInfo[] DefinedMethods => Type.GetMethods();
    internal MethodInfo DefinedMethod => DefinedMethods[0];

    internal SolutionContainer(Type type)
    {
        Type = type;
    }
}
