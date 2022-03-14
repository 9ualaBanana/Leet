using System.Collections;
using System.Text;

namespace CCEasy.Services;

internal class SolutionResultPresenter
{
    Stream _outputStream = Console.OpenStandardOutput();
    internal Stream OutputStream
    {
        get => _outputStream;
        set
        {
            _resultWriter = null;
            _outputStream = value;
        }
    }
    StreamWriter? _resultWriter;
    StreamWriter _ResultWriter => _resultWriter ??= new(OutputStream);

    internal void DisplayResults(object? expected, object? actual)
    {
        if (_ResultWriter.BaseStream.CanSeek) _ResultWriter.BaseStream.Seek(0, SeekOrigin.End);
        _ResultWriter.WriteLine($"{"Expected:", -10} {GetDisplayableRepresentation(expected)}");
        _ResultWriter.WriteLine($"{"Actual:", -10} {GetDisplayableRepresentation(actual)}");
        _ResultWriter.WriteLine();
        _ResultWriter.Close();
    }

    static string GetDisplayableRepresentation(object? value) => value switch
    {
        null => "null",
        IEnumerable enumerable => GetDisplayableEnumerable(enumerable),
        _ => value.ToString()!
    };

    static string GetDisplayableEnumerable(IEnumerable sequence)
    {
        var length = 0;
        foreach (var element in sequence) length++;

        var displayableSequence = new StringBuilder();
        displayableSequence.Append("[ ");
        foreach (var element in sequence)
        {
            var displayableElement = GetDisplayableRepresentation(element);
            displayableSequence.Append(displayableElement);

            if (length == 1) break;
            displayableSequence.Append(", ");
            length--;
        }
        displayableSequence.Append(" ]");
        return displayableSequence.ToString();
    }
}
