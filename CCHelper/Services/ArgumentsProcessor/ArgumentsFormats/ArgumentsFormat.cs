namespace CCHelper.Services.ArgumentsProcessing.ArgumentsFormats;

internal abstract class ArgumentsFormat
{
    bool _matched;

    internal bool Match(object?[]? arguments)
    {
        return _matched = MatchImpl(arguments);
    }
    protected abstract bool MatchImpl(object?[]? arguments);

    internal void Normalize(ref object?[]? arguments)
    {
        if (!_matched) throw new InvalidOperationException("Arguments must match format.");
        NormalizeImpl(ref arguments);
    }
    protected abstract void NormalizeImpl(ref object?[]? arguments);
}
