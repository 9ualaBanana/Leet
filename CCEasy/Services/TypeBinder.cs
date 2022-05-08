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

internal static class TypeBinder
{
    internal static bool ArgumentCanBindToParameter(object? argument, ParameterInfo parameter)
    {
        var argumentType = argument?.GetType();
        var parameterType = parameter.ParameterType;

        return CanBind(argumentType, parameterType);
    }

    /// <summary>
    /// The constraint that satisfies both reference and nullable types doesn't exist, so this method makes up for it.
    /// </summary>
    /// <remarks>
    /// Provides a little more descriptive exception.
    /// </remarks>
    /// <exception cref="ArgumentException">The contstraint is not satisfied.</exception>
    internal static TResult Cast<TResult>(object? obj)
    {
        var objectType = obj?.GetType();
        // Conversion of `null` to `TResult` won't throw if the types are compatible.
        if (CanBind(objectType, typeof(TResult))) return (TResult)obj!;

        var typeInfo = obj is null ? "[null]" : $"<{objectType}>";
        throw new ArgumentException($"{typeInfo} value " +
            $"is not compatible with the provided type parameter <{typeof(TResult)}>.");
    }

    internal static bool CanBind(Type? originType, Type targetType)
    {
        if (originType is null) return CanHoldNull(targetType);
        return originType.IsAssignableTo(targetType);
    }

    internal static bool CanHoldNull(Type parameterType)
    {
        return !parameterType.IsValueType || Nullable.GetUnderlyingType(parameterType) is not null;
    }
}
