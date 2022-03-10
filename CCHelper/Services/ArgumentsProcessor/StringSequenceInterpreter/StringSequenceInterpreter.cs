using System.Text.RegularExpressions;

namespace CCHelper.Services.ArgumentsProcessor.StringSequenceInterpreter;

internal class StringSequenceInterpreter
{
    readonly string _stringSequence;
    readonly Brackets _brackets;
    const string _elementsCapturingGroup = "elements";

    Regex SequenceRegex => new(
        $@"\{_brackets.OpeningBracket}\s*(?<{_elementsCapturingGroup}>(?:[-+]?\d{{1,}},\s*)*[-+]?\d{{1,}})\s*\{_brackets.ClosingBracket}"
        );
    MatchCollection? _parsedOutStringSequences;
    MatchCollection ParsedOutStringSequences
    {
        get
        {
            _parsedOutStringSequences ??= SequenceRegex.Matches(_stringSequence);
            if (_parsedOutStringSequences.Any()) return _parsedOutStringSequences;

            throw new ArgumentException("The string sequence has invalid format.", nameof(_stringSequence));
        }
    }

    internal StringSequenceInterpreter(string stringSequence)
    {
        _stringSequence = stringSequence.Trim();
        _brackets = RetrieveBracketsFromStringSequence();
    }
    Brackets RetrieveBracketsFromStringSequence()
    {
        var brackets = new Brackets(_stringSequence.First(), _stringSequence.Last());
        if (brackets.AreSupported) return brackets;

        throw new ArgumentException("Exception occured when trying to retrieve brackets.", nameof(_stringSequence));
    }

    internal int[] ToArray()
    {
        return ToEnumerable().ToArray();
    }

    internal IEnumerable<int> ToEnumerable()
    {
        var parsedOutStringSequence = ParsedOutStringSequences.Single();
        return InterpretSequence(parsedOutStringSequence);
    }

    internal int[][] ToJaggedArray()
    {
        var jaggedArray = new int[ParsedOutStringSequences.Count][];
        FillJaggedArray(jaggedArray);
        return jaggedArray;
    }
    void FillJaggedArray(int[][] jaggedArray)
    {
        var dimension = 0;
        foreach (Match parsedOutStringSequence in ParsedOutStringSequences)
        {
            jaggedArray[dimension] = InterpretSequence(parsedOutStringSequence).ToArray();
            dimension++;
        }
    }

    static IEnumerable<int> InterpretSequence(Match parsedOutStringSequence)
    {
        return CastStringElementsToInt(GetStringSequenceElements(parsedOutStringSequence));
    }

    static string[] GetStringSequenceElements(Match parsedOutStringSequence)
    {
        return SplitUnwrappedElements(parsedOutStringSequence.Groups[_elementsCapturingGroup].Value);
    }

    static string[] SplitUnwrappedElements(string unwrappedStringSequence)
    {
        return unwrappedStringSequence.Split(',', StringSplitOptions.TrimEntries);
    }

    static IEnumerable<int> CastStringElementsToInt(string[] stringElements)
    {
        return stringElements.Select(stringElement => int.Parse(stringElement));
    }
}
