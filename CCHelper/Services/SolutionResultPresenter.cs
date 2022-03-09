using System.Collections;
using System.Text;

namespace CCHelper.Services;

internal class SolutionResultPresenter
{
    readonly object _expectedResult;
    readonly object _actualResult;

    internal SolutionResultPresenter(object expectedResult, object actualResult)
    {
        _expectedResult = expectedResult;
        _actualResult = actualResult;
    }

    internal void DisplayResults()
    {
        Console.WriteLine($"{"Expected:", -10} {GetDisplayable(_expectedResult!)}");
        Console.WriteLine($"{"Actual:", -10} {GetDisplayable(_actualResult!)}");
    }
    
    string GetDisplayable(object value)
    {
        Guard.Against.Null(value, nameof(value));

        if (value is not IEnumerable) return value.ToString()!;
        
        return GetDisplayableSequence((IEnumerable)value); ;
    }
    string GetDisplayableSequence(IEnumerable sequence)
    {
        var displayableSequence = new StringBuilder();
        displayableSequence.Append("[ ");
        foreach (var element in sequence)
        {
            var displayableElement = element is IEnumerable subSequence ? GetDisplayableSequence(subSequence) : element;
            displayableSequence.Append(displayableElement);
            displayableSequence.Append(", ");
        }
        RemoveLastSeparator(displayableSequence);
        displayableSequence.Append(" ]");
        return displayableSequence.ToString();
    }
    static void RemoveLastSeparator(StringBuilder displayableSequence) => displayableSequence.Remove(displayableSequence.Length - 2, 2);
}
