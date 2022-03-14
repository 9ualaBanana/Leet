using CCEasy.Test.Framework.Abstractions.SolutionMethod;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CCEasy.Test.Framework.Abstractions.SolutionContext;

public class SolutionContextProvider
{
    readonly TypeBuilder _solutionContainerBuilder;

    SolutionContainer? _solutionContainer;
    /// <summary>
    /// Once this property is accessed no additional methods can be defined inside the context.
    /// </summary>
    internal SolutionContainer SolutionContainer => _solutionContainer ??= BuildSolutionContainer();

    /// <summary>
    /// Runtime builder of a solution context (i.e. dynamic assembly and module with <see cref="SolutionContainer"/> in it <br/>
    /// that is a type for defining <see cref="SolutionMethodStub"/> built at runtime).
    /// </summary>
    internal SolutionContextProvider()
    {
        _solutionContainerBuilder = NewDynamicEnvironment.DefineType("SolutionContainer");
    }
    static ModuleBuilder NewDynamicEnvironment
    {
        get
        {
            var assembly = AssemblyBuilder.DefineDynamicAssembly(new(Guid.NewGuid().ToString()), AssemblyBuilderAccess.RunAndCollect);
            return assembly.DefineDynamicModule(Guid.NewGuid().ToString());
        }
    }

    SolutionContainer BuildSolutionContainer()
    {
        return new(_solutionContainerBuilder.CreateType()!);
    }

    /// <summary>
    /// Define the provided <see cref="SolutionMethodStub"/> implementation inside the dynamically created context.
    /// </summary>
    /// <param name="solutionMethodStub">the data object to define inside the context.</param>
    internal void DefineMethod(SolutionMethodStub solutionMethodStub)
    {
        var solutionMethodBuilder = DefineMethodSignature(solutionMethodStub);
        ApplyLabels(solutionMethodBuilder, solutionMethodStub);
        DefineReturnStatement(solutionMethodBuilder, solutionMethodStub.ReturnType!);
    }
    MethodBuilder DefineMethodSignature(SolutionMethodStub solutionMethodStub)
    {
        return _solutionContainerBuilder.DefineMethod(
            solutionMethodStub.Name,
            solutionMethodStub.AccessModifier,
            CallingConventions.HasThis,
            solutionMethodStub.ReturnType,
            solutionMethodStub.Parameters
            );
    }
    // Refactor label applying logic to be used through the same interface. So I could just ApplyLabel or something and it would work.
    static void ApplyLabels(MethodBuilder solutionMethodBuilder, SolutionMethodStub solutionMethodStub)
    {
        if (solutionMethodStub.HasSolutionLabel)
        {
            ApplySolutionLabel(solutionMethodBuilder);
        }
        if (solutionMethodStub.ResultAttributesPositions is not null)
        {
            ApplyResultLabels(solutionMethodBuilder, solutionMethodStub.ResultAttributesPositions);
        }
    }
    static void ApplySolutionLabel(MethodBuilder solutionMethodBuilder)
    {
        var solutionAttributeConstructor = typeof(SolutionAttribute).GetConstructor(Type.EmptyTypes)!;
        var solutionAttribute = new CustomAttributeBuilder(solutionAttributeConstructor, Array.Empty<object>());
        solutionMethodBuilder.SetCustomAttribute(solutionAttribute);
    }
    static void ApplyResultLabels(MethodBuilder solutionMethodBuilder, int[] parametersPositions)
    {
        var resultAttributeConstructor = typeof(ResultAttribute).GetConstructor(Type.EmptyTypes)!;
        var resultAttribute = new CustomAttributeBuilder(resultAttributeConstructor, Array.Empty<object>());
        foreach (var position in parametersPositions)
        {
            solutionMethodBuilder.DefineParameter(position, ParameterAttributes.None, null).SetCustomAttribute(resultAttribute);
        }
    }
    static void DefineReturnStatement(MethodBuilder solutionMethodBuilder, Type returnType)
    {
        var ilGenerator = solutionMethodBuilder.GetILGenerator();
        if (returnType != typeof(void)) PushReturnValueOnStack(ilGenerator);
        ilGenerator.Emit(OpCodes.Ret);
    }
    static void PushReturnValueOnStack(ILGenerator ilGenerator)
    {
        ilGenerator.Emit(OpCodes.Newobj, typeof(object).GetConstructor(Type.EmptyTypes)!);
    }
}
