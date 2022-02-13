using System.Reflection;

namespace CCHelper;

internal static class SolutionMethodValidator
{
    internal static MethodInfo DetectSolutionMethod(this IEnumerable<MethodInfo> methods)
    {
        var validSolutionMethods = methods.Where(IsValid);

        if (!validSolutionMethods.Any()) throw new EntryPointNotFoundException("Solution method was not found inside the provided solution container.");
        if (validSolutionMethods.Count() > 1) throw new AmbiguousMatchException("Solution container must contain exactly one solution method.");

        return validSolutionMethods.Single();
    }
    static bool IsValid(MethodInfo method)
    {
        bool isValidReturnValueSolutionMethod = method.IsValidReturnValueSolutionMethod();
        bool isValidResultArgumentSolutionMethod = method.IsValidResulArgumentSolutionMethod();

        return isValidReturnValueSolutionMethod ^ isValidResultArgumentSolutionMethod;
    }
    internal static bool IsValidReturnValueSolutionMethod(this MethodInfo method)
    {
        bool hasSolutionAttribute = method.HasSolutionAttribute();
        bool hasResultAttribute = method.HasResultAttribute();
        bool hasCorrectReturnType = method.ReturnType != typeof(void);

        if (hasSolutionAttribute && hasResultAttribute) throw new AmbiguousMatchException("Solution method must be labeled with exactly one attribute.");
        if (hasSolutionAttribute && !hasCorrectReturnType) throw new InvalidOperationException("Method labeled with [Solution] can't return void.");

        return hasSolutionAttribute && hasCorrectReturnType;
    }
    internal static bool IsValidResulArgumentSolutionMethod(this MethodInfo method)
    {
        int resultAttributesCount = method.GetParameters().
            Where(parameter => parameter.CustomAttributes.
            Any(attribute => attribute.AttributeType == typeof(ResultAttribute))).Count();
        // No check for parameters presence is needed, as [Result] attribute can only be applied to parameters and that implies there is at least one.
        bool hasSolutionAttribute = method.HasSolutionAttribute();
        bool hasCorrectReturnType = method.ReturnType == typeof(void);

        if (hasSolutionAttribute && resultAttributesCount > 0) throw new AmbiguousMatchException("Solution method must be labeled with exactly one attribute.");
        if (resultAttributesCount > 1) throw new AmbiguousMatchException("Multiple [Result] attributes are not allowed.");
        if (resultAttributesCount > 0 && !hasCorrectReturnType) throw new InvalidOperationException("Method labeled with [Result] must return void.");

        return resultAttributesCount == 1;
    }
    internal static bool HasSolutionAttribute(this MethodInfo method)
    {
        return method.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(SolutionAttribute));
    }
    internal static bool HasResultAttribute(this MethodInfo method)
    {
        return method.GetParameters().
            Any(parameter => parameter.CustomAttributes.
            Any(attribute => attribute.AttributeType == typeof(ResultAttribute)));
    }
}