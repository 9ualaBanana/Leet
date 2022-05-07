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

using System.Reflection;

namespace CCEasy.Services;

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