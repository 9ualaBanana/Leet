using System.Reflection;

namespace CCHelper;

// Probably it's not used that much (only from the factory currently) to be instantiated as global static Singleton.
// I mean the factory or any other client will be able to create it as a Transient service when needed.
internal static class SolutionMethodValidator
{
    readonly static Predicate<MethodInfo>[] _validators = new Predicate<MethodInfo>[]
    {
        IsValidOutputSolution,
        IsValidInputSolution
    };

    internal static IEnumerable<MethodInfo> RetrieveValidSolutionMethods(this object container) => container.GetType().GetMethods().Where(IsValidSolutionMethod);
    static bool IsValidSolutionMethod(MethodInfo method)
    {
        return _validators.Any(isValid => isValid(method));
    }

    internal static bool IsValidOutputSolution(this MethodInfo method)
    {
        bool hasSolutionAttribute = method.IsOutputSolution();
        bool hasResultAttribute = method.IsInputSolution();
        bool hasCorrectReturnType = method.ReturnType != typeof(void);

        if (hasSolutionAttribute && hasResultAttribute) throw new AmbiguousMatchException("Solution method must be labeled with exactly one attribute.");
        if (hasSolutionAttribute && !hasCorrectReturnType) throw new FormatException("Method labeled with [Solution] can't return void.");

        return hasSolutionAttribute && hasCorrectReturnType;
    }
    internal static bool IsOutputSolution(this MethodInfo method)
    {
        return method.IsDefined(typeof(SolutionAttribute));
    }

    internal static bool IsValidInputSolution(this MethodInfo method)
    {
        int resultAttributesCount = method.GetParameters().
            Where(parameter => parameter.CustomAttributes.
            Any(attribute => attribute.AttributeType == typeof(ResultAttribute))).Count();
        // No check for parameters presence is needed, as [Result] attribute can only be applied to parameters and that implies there is at least one.
        bool hasSolutionAttribute = method.IsOutputSolution();
        bool hasCorrectReturnType = method.ReturnType == typeof(void);

        if (hasSolutionAttribute && resultAttributesCount > 0) throw new AmbiguousMatchException("Solution method must be labeled with exactly one attribute.");
        if (resultAttributesCount > 1) throw new AmbiguousMatchException("Multiple [Result] attributes are not allowed.");
        if (resultAttributesCount > 0 && !hasCorrectReturnType) throw new FormatException("Method labeled with [Result] must return void.");

        return resultAttributesCount == 1;
    }
    internal static bool IsInputSolution(this MethodInfo method)
    {
        return method.GetParameters().Any(parameter => parameter.IsDefined(typeof(ResultAttribute)));
    }
}