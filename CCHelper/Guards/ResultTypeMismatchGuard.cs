internal static class ResultTypeMismatchGuard
{
    internal static void ResultTypeMismatch<TResult>(this IGuardClause guardClause, object result, string parameterName)
    {
        if (result.GetType() != typeof(TResult))
        {
            throw new InvalidCastException($"Type provided in place of {nameof(TResult)} ({typeof(TResult)})" +
                $" doesn't match the actual result type of the solution method ({result.GetType()})");
        }
    }
}