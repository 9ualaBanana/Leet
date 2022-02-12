using System.Reflection;

namespace CCHelper;

internal static class SolutionMethodFactory
{
    internal static SolutionMethod Detect(object solutionContainer)
    {
        var potentialSolutionMethods = solutionContainer.GetType().GetMethods().Where(method => !SolutionMethod.IsMalformed(method));
        var solutionMethod = potentialSolutionMethods.Single();
        if (IsLabeledWithResult(solutionMethod)) return new ResultArgumentSolutionMethod(solutionMethod, solutionContainer);
        if (IsLabeledWithSolution(solutionMethod)) return new ReturnValueSolutionMethod(solutionMethod, solutionContainer);
        throw new ApplicationException("Something went wrong when trying to detect the solution method.");
    }

    static bool IsLabeledWithSolution(MethodInfo method)
    {
        return method.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(SolutionAttribute));
    }

    static bool IsLabeledWithResult(MethodInfo method)
    {
        return method.GetParameters().Any(parameter => parameter.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(ResultAttribute)));
    }
}