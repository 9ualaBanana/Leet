using System.Reflection;

namespace CCHelper.Services;

internal static class TypeBinder
{
    internal static bool ArgumentCanBindToParameter(object? argument, ParameterInfo parameter)
    {
        var argumentType = argument?.GetType();
        var parameterType = parameter.ParameterType;

        return CanBind(argumentType, parameterType);
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
