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

namespace Leet.Services.StringInterpreter;

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
