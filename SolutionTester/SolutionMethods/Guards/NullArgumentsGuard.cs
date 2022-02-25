internal static class NullArgumentsGuard
{
    internal static object[] NullArguments(this IGuardClause guardClause, object?[]? arguments, string parameterName)
    {
        if (arguments is null || arguments.Any(argument => argument is null))
        {
            throw new ArgumentNullException(parameterName, "null arguments are not supported.");
        }

        return arguments!;
    }
}