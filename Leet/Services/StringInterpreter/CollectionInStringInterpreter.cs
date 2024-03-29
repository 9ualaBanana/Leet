﻿//MIT License

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

using System.Text.RegularExpressions;

namespace Leet.Services.StringInterpreter;

/// <summary>
/// Provides the means for retrieving objects derived from <see cref="IEnumerable{T}"/> represented as <see cref="string"/>.
/// </summary>
/// <typeparam name="TInterpreted">The type of elements of the resulting object derived from <see cref="IEnumerable{T}"/>.</typeparam>
public class CollectionInStringInterpreter<TInterpreted>
{
    readonly string _collectionInString;
    readonly Brackets _brackets;
    const string _elementsCapturingGroup = "elements";
    readonly Func<string, TInterpreted> _interpreter;

    Regex? _collectionRegex;
    Regex CollectionRegex => _collectionRegex ??= new(
        $@"\{_brackets.OpeningBracket}\s*
        (?<{_elementsCapturingGroup}>
            (
                (?<element>[^\{_brackets.OpeningBracket}\s\{_brackets.ClosingBracket}]+?)
                (?<separator>\s*,\s*)?
            )+  # Wraps elemenets as an integral whole.
        )
        \s*\{_brackets.ClosingBracket}",
        RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture
        );
    MatchCollection? _parsedOutCollectionsInString;
    MatchCollection ParsedOutCollectionsInString
    {
        get
        {
            _parsedOutCollectionsInString ??= CollectionRegex.Matches(_collectionInString);
            if (_parsedOutCollectionsInString.Any()) return _parsedOutCollectionsInString;

            throw new ArgumentException("The collection in string has invalid format.", nameof(_collectionInString));
        }
    }

    /// <summary>
    /// Instantiates the <see cref="CollectionInStringInterpreter{TInterpreted}"/> object that allows retrieving
    /// objects derived from <see cref="IEnumerable{T}"/> represented by <paramref name="collectionInString"/>.
    /// </summary>
    /// <param name="collectionInString">The <see cref="string"/> representing <see cref="IEnumerable{T}"/> in text form.</param>
    /// <param name="interpreter">The delegate used for casting the resulting elements to <typeparamref name="TInterpreted"/>.</param>
    public CollectionInStringInterpreter(string collectionInString, Func<string, TInterpreted> interpreter)
    {
        _collectionInString = collectionInString.Trim();
        _brackets = RetrieveBracketsFromCollectionInString();
        _interpreter = interpreter;
    }
    Brackets RetrieveBracketsFromCollectionInString()
    {
        var brackets = new Brackets(_collectionInString.First(), _collectionInString.Last());
        if (brackets.AreSupported) return brackets;

        throw new ArgumentException("Exception occured when trying to retrieve brackets.", nameof(_collectionInString));
    }

    /// <summary>
    /// Tries to perform in-place conversion of the enumerable represented as <see cref="string"/> inside <paramref name="argument"/>
    /// to a valid C# collection<br/> applying <paramref name="interpreter"/> to each element.
    /// </summary>
    /// <returns><c>true</c> if the conversion succeeded; <c>false</c> otherwise.</returns>
    public static bool TryInterpret(ref object? argument, Func<string, TInterpreted> interpreter)
    {
        if (argument?.GetType() != typeof(string)) return false;

        try
        {
            argument = new CollectionInStringInterpreter<TInterpreted>(argument.ToString()!, interpreter).AppropriateInterpreter();
        }
        catch (Exception) { return false; }
        return true;
    }

    Func<object> AppropriateInterpreter => Dimensions > 1 ? ToJaggedArray : ToArray;

    int _dimensions;
    int Dimensions
    {
        get
        {
            if (_dimensions != 0) return _dimensions;

            foreach (var symbol in _collectionInString)
            {
                if (symbol == _brackets.OpeningBracket || char.IsWhiteSpace(symbol))
                {
                    if (symbol == _brackets.OpeningBracket) _dimensions++;
                    continue;
                }
                break;
            }
            return _dimensions != 0 ?
                _dimensions :
                throw new ArgumentException("Exception occured when trying to count dimensions.", nameof(_collectionInString));
        }
    }

    /// <returns>The array retrieved from the <see cref="string"/>.</returns>
    public TInterpreted[] ToArray()
    {
        return ToEnumerable().ToArray();
    }

    /// <returns>The enumerable retrieved from the <see cref="string"/>.</returns>
    public IEnumerable<TInterpreted> ToEnumerable()
    {
        var parsedOutCollectionInString = ParsedOutCollectionsInString.Single();
        return InterpretCollection(parsedOutCollectionInString);
    }

    /// <remarks>
    /// If the <see cref="string"/> represents a regular array, the resulting object will be a jagged array
    /// with the first element being that retrieved array.
    /// </remarks>
    /// <returns>The jagged array retrieved from the <see cref="string"/>.</returns>
    public TInterpreted[][] ToJaggedArray()
    {
        var jaggedArray = new TInterpreted[ParsedOutCollectionsInString.Count][];
        FillJaggedArray(jaggedArray);
        return jaggedArray;
    }
    void FillJaggedArray(TInterpreted[][] jaggedArray)
    {
        var dimension = 0;
        foreach (Match parsedOutCollectionInString in ParsedOutCollectionsInString)
        {
            jaggedArray[dimension] = InterpretCollection(parsedOutCollectionInString).ToArray();
            dimension++;
        }
    }

    IEnumerable<TInterpreted> InterpretCollection(Match parsedOutStringSequence)
    {
        return CastStringElements(GetCollectionInStringElements(parsedOutStringSequence));
    }

    string[] GetCollectionInStringElements(Match parsedOutCollectionInString)
    {
        return SplitUnwrappedElements(parsedOutCollectionInString.Groups[_elementsCapturingGroup].Value);
    }

    string[] SplitUnwrappedElements(string unwrappedCollectionInString)
    {
        return unwrappedCollectionInString.Split(',', StringSplitOptions.TrimEntries);
    }

    IEnumerable<TInterpreted> CastStringElements(string[] stringElements)
    {
        try
        {
            return stringElements.Select(stringElement => _interpreter(stringElement));
        }
        catch (FormatException castException)
        {
            throw new InvalidCastException("The exception occurred trying to cast elements of the collection in string " +
                "using the provided interpreter.", castException);
        }
    }
}
