namespace CCHelper.Services.ArgumentsProcessor;

internal interface IArgumentsFormat
{
    bool Match(object?[]? arguments);
    void Normalize(ref object?[]? arguments);
}
