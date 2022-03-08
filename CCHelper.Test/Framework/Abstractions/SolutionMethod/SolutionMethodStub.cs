using CCHelper.Test.Framework.Abstractions.SolutionContext;
using System;
using System.Reflection;

namespace CCHelper.Test.Framework.Abstractions.SolutionMethod;

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
