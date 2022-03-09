namespace CCHelper.Services.ArgumentsProcessor;

/// <summary>
/// The single <c>null</c> argument or the argument passed as an array (regular or jagged)
/// got unwrapped by <c>params object[]</c>.
/// </summary>
/// <remarks>
/// Restores the actual number of passed arguments.
/// </remarks>
internal class UnwrappedArguments : IArgumentsFormat
{
    public bool Match(object?[]? arguments)
    {
        return arguments is null || arguments.GetType() != typeof(object[]);
    }
    public void Normalize(ref object?[]? arguments)
    {
        arguments = new object?[] { arguments };
    }
}
