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
            _ResultWriter.Close();
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
        _ResultWriter.Flush();
    }

    static string GetDisplayableRepresentation(object? value) => value switch
    {
        null => "null",
        string string_ => "\"" + string_ + "\"",
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

    ~SolutionResultPresenter()
    {
        _ResultWriter.Close();
    }
}
