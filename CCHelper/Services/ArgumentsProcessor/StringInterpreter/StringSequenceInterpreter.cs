using System.Text.RegularExpressions;

namespace CCHelper.Services.ArgumentsProcessing.StringInterpreter;

public class StringSequenceInterpreter<TInterpreted>
{
    readonly string _stringSequence;
    readonly Brackets _brackets;
    const string _elementsCapturingGroup = "elements";
    readonly Func<string, TInterpreted> _interpreter;

    Regex? _sequenceRegex;
    Regex SequenceRegex => _sequenceRegex ??= new(
        $@"\{_brackets.OpeningBracket}\s*
        (?<{_elementsCapturingGroup}>
            (
                (?<digit>[-+]?( \d+ | \.\d+ ))  # To allow nulls, the casting step requires additional check for nulls as well.
                (?<separator>\s*,\s*)?
            )+  # Wraps elemenets as an integral whole.
        )
        \s*{_brackets.ClosingBracket}",
        RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture
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
    // Define either the additional parameter for the separator with the default value or a separate constructor altogether.
    public StringSequenceInterpreter(string stringSequence, Func<string, TInterpreted> interpreter)
    {
        _stringSequence = stringSequence.Trim();
        _brackets = RetrieveBracketsFromStringSequence();
        _interpreter = interpreter;
    }
    Brackets RetrieveBracketsFromStringSequence()
    {
        var brackets = new Brackets(_stringSequence.First(), _stringSequence.Last());
        if (brackets.AreSupported) return brackets;

        throw new ArgumentException("Exception occured when trying to retrieve brackets.", nameof(_stringSequence));
    }

    public Func<object> AppropriateInterpreter => Dimensions > 1 ? ToJaggedArray : ToArray;

    int _dimensions;
    int Dimensions
    {
        get
        {
            if (_dimensions != 0) return _dimensions;

            foreach (var symbol in _stringSequence)
            {
                if (symbol == _brackets.OpeningBracket || symbol == ' ')
                {
                    if (symbol == _brackets.OpeningBracket) _dimensions++;
                    continue;
                }
                break;
            }
            return _dimensions != 0 ?
                _dimensions :
                throw new ArgumentException("Exception occured when trying to count dimensions.", nameof(_stringSequence));
        }
    }

    public TInterpreted[] ToArray()
    {
        return ToEnumerable().ToArray();
    }

    public IEnumerable<TInterpreted> ToEnumerable()
    {
        var parsedOutStringSequence = ParsedOutStringSequences.Single();
        return InterpretSequence(parsedOutStringSequence);
    }

    public TInterpreted[][] ToJaggedArray()
    {
        var jaggedArray = new TInterpreted[ParsedOutStringSequences.Count][];
        FillJaggedArray(jaggedArray);
        return jaggedArray;
    }
    void FillJaggedArray(TInterpreted[][] jaggedArray)
    {
        var dimension = 0;
        foreach (Match parsedOutStringSequence in ParsedOutStringSequences)
        {
            jaggedArray[dimension] = InterpretSequence(parsedOutStringSequence).ToArray();
            dimension++;
        }
    }

    IEnumerable<TInterpreted> InterpretSequence(Match parsedOutStringSequence)
    {
        return CastStringElements(GetStringSequenceElements(parsedOutStringSequence));
    }

    string[] GetStringSequenceElements(Match parsedOutStringSequence)
    {
        return SplitUnwrappedElements(parsedOutStringSequence.Groups[_elementsCapturingGroup].Value);
    }

    string[] SplitUnwrappedElements(string unwrappedStringSequence)
    {
        return unwrappedStringSequence.Split(',', StringSplitOptions.TrimEntries);
    }

    IEnumerable<TInterpreted> CastStringElements(string[] stringElements)
    {
        return stringElements.Select(stringElement => _interpreter(stringElement));
    }
}
