namespace CCHelper.Services.ArgumentsProcessing.ArgumentsFormats;

/// <summary>
/// The single <c>null</c> argument or the argument passed as a jagged array
/// got unwrapped by <c>params object[]</c>.
/// </summary>
/// <remarks>
/// Restores the actual number of passed arguments.
/// </remarks>
internal class UnwrappedArgumentsFormat : ArgumentsFormat
{
    protected override bool MatchImpl(object?[]? arguments)
    {
        return arguments is null || arguments.GetType() != typeof(object[]);
    }
    protected override void NormalizeImpl(ref object?[]? arguments)
    {
        arguments = new object?[] { arguments };
    }
}
