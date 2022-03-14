namespace CCEasy.Services.ArgumentsProcessing.StringInterpreter;

internal record Brackets
{
    internal char OpeningBracket { get; private set; }
    internal char ClosingBracket { get; private set; }

    internal Brackets(BracketsType bracketsType)
    {
        switch (bracketsType)
        {
            case BracketsType.Angle:
                Init('<', '>');
                break;
            case BracketsType.Curly:
                Init('{', '}');
                break;
            case BracketsType.Round:
                Init('(', ')');
                break;
            case BracketsType.Square:
                Init('[', ']');
                break;
            default:
                throw new NotImplementedException();
        }
    }
    internal Brackets(char openingBracket, char closingBracket)
    {
        Init(openingBracket, closingBracket);
    }
    void Init(char openingBracket, char closingBracket)
    {
        OpeningBracket = openingBracket;
        ClosingBracket = closingBracket;
    }

    internal bool AreSupported
    {
        get
        {
            Brackets supportedBrackets;
            foreach (var bracketsType in Enum.GetValues(typeof(BracketsType)))
            {
                supportedBrackets = new Brackets((BracketsType)bracketsType);
                if (this == supportedBrackets) return true;
            }
            return false;
        }
    }
}
