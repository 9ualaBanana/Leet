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
