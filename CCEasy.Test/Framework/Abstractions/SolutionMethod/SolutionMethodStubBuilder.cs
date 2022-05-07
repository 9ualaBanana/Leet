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

using Ardalis.GuardClauses;
using CCEasy.Test.Framework.Abstractions.SolutionContext;
using System;
using System.Reflection;

namespace CCEasy.Test.Framework.Abstractions.SolutionMethod;

internal class SolutionMethodStubBuilder
{
    SolutionMethodStub _product;

    internal SolutionMethodStubBuilder()
    {
        _product = new();
    }

    internal SolutionMethodStubBuilder WithAccessModifier(AccessModifier accessModifier)
    {
        _product.AccessModifier = (MethodAttributes)accessModifier;
        return this;
    }

    internal SolutionMethodStubBuilder Accepting(params Type[] parameterTypes)
    {
        _product.Parameters = parameterTypes;
        return this;
    }

    internal SolutionMethodStubBuilder Returning(Type returnType)
    {
        _product.ReturnType = returnType;
        return this;
    }

    internal SolutionMethodStubBuilder WithSolutionLabel
    {
        get
        {
            _product.HasSolutionLabel = true;
            return this;
        }
    }

    /// <remarks>
    /// Also sets the appopriate for <see cref="InputSolution{TResult}"/> return type.
    /// </remarks>
    /// <param name="parameterPositions">the positions of parameters (starting from 1) to which the label will be applied .</param>
    internal SolutionMethodStubBuilder WithResultLabelAppliedToParameter(params int[] parameterPositions)
    {
        Guard.Against.Null(_product.Parameters,
            nameof(_product.Parameters), "Formal parameters must be provided before applying attributes to them.");

        _product.ResultAttributesPositions = parameterPositions;
        return this;
    }

    /// <summary>
    /// Creates the definition for <see cref="SolutionMethodStub"/> being constructed
    /// inside <paramref name="contextProvider"/>.
    /// </summary>
    /// <param name="contextProvider">the builder of a runtime context.</param>
    internal void PutInContext(SolutionContextProvider contextProvider)
    {
        var solutionMethodStub = Build();
        contextProvider.DefineMethod(solutionMethodStub);
    }

    internal SolutionMethodStub Build()
    {
        Guard.Against.Null(_product.ReturnType, nameof(_product.ReturnType));

        return _product;
    }
}
