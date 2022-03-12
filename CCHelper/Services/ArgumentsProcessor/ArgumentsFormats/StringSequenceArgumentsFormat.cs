using CCHelper.Services.ArgumentsProcessing.StringInterpreter;

namespace CCHelper.Services.ArgumentsProcessing.ArgumentsFormats;

/// <summary>
/// The single <see cref="string"/> representing a sequence enclosed in brackets.
/// </summary>
/// <remarks>
/// Converts the sequence represented by the string to a valid C# collection.
/// </remarks>
internal class StringSequenceArgumentsFormat : ArgumentsFormat
{
    protected override bool MatchImpl(object?[]? arguments)
    {
        if (arguments is null || arguments.Length != 1) return false;
        return arguments.First()?.GetType() == typeof(string);
    }

    protected override void NormalizeImpl(ref object?[]? arguments)
    {
        arguments![0] = new StringSequenceInterpreter(arguments!.First()!.ToString()!).AppropriateInterpreter();
    }
}
