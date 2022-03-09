using System.Reflection;

namespace CCHelper.Services;

internal static class SolutionMethodValidator
{
    // These validators need to know about each other as they contain the logic that requires that
    // to be able to throw descriptive exceptions and it can't be done independetly outside of them.
    // A validator simply needs to be added to this array as well as some mebmer that differentiates
    // a particular concrete implementation among others needs to be introduced, to be the part of the system
    // that is taken into account in the process of discovering and upon construction respectively.
    readonly static Predicate<MethodInfo>[] _validators =
    {
        IsValidOutputSolution,
        IsValidInputSolution
    };

    internal static bool IsValidSolutionMethod(MethodInfo methodInfo)
    {
        return _validators.Any(isValid => isValid(methodInfo));
    }

    static bool IsValidOutputSolution(this MethodInfo methodInfo)
    {
        bool hasSolutionLabel = methodInfo.HasSolutionLabel();
        bool hasResultLabel = methodInfo.HasResultLabel();
        bool hasCorrectReturnType = methodInfo.ReturnType != typeof(void);

        if (hasSolutionLabel && hasResultLabel) throw new AmbiguousMatchException("Solution method must be labeled with exactly one attribute.");
        if (hasSolutionLabel && !hasCorrectReturnType) throw new FormatException("Method labeled with [Solution] can't return void.");

        return hasSolutionLabel && hasCorrectReturnType;
    }
    internal static bool HasSolutionLabel(this MethodInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(SolutionAttribute));
    }

    static bool IsValidInputSolution(this MethodInfo methodInfo)
    {
        int resultLabelsCount = methodInfo.GetParameters().
            Where(parameter => parameter.CustomAttributes.
            Any(attribute => attribute.AttributeType == typeof(ResultAttribute))).Count();
        // No check for parameters presence is needed, as [Result] attribute can only be applied to parameters and that implies there is at least one.
        bool hasSolutionLabel = methodInfo.HasSolutionLabel();
        bool hasCorrectReturnType = methodInfo.ReturnType == typeof(void);

        if (hasSolutionLabel && resultLabelsCount > 0) throw new AmbiguousMatchException("Solution method must be labeled with exactly one attribute.");
        if (resultLabelsCount > 1) throw new AmbiguousMatchException("Multiple [Result] attributes are not allowed.");
        if (resultLabelsCount > 0 && !hasCorrectReturnType) throw new FormatException("Method labeled with [Result] must return void.");

        return resultLabelsCount == 1;
    }
    internal static bool HasResultLabel(this MethodInfo methodInfo)
    {
        return methodInfo.GetParameters().Any(parameter => parameter.IsDefined(typeof(ResultAttribute)));
    }
}