using System;
using System.Reflection;

namespace CCHelper.Test.Framework.Abstractions.SolutionContext;

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
