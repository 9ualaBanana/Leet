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

using Leet.Services.StringInterpreter;
using Leet.Test.Framework.TestData;
using System;
using Xunit;

namespace Leet.Test.Tests.Units;

public class TestCollectionInStringInterpreter
{
    CollectionInStringInterpreter<TInterpreted> SUT_CollectionInStringInterpreter<TInterpreted>(
        string collectionInString,
        Func<string, TInterpreted> interpreter
        )
    {
        return new(collectionInString, interpreter);
    }



    [Theory]
    [MemberData(nameof(CollectionInStringData.Erroneous), MemberType = typeof(CollectionInStringData))]
    public void ToEnumerable_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_CollectionInStringInterpreter(stringWithBrackets, int.Parse).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Erroneous), MemberType = typeof(CollectionInStringData))]
    public void ToArray_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_CollectionInStringInterpreter(stringWithBrackets, int.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Erroneous), MemberType = typeof(CollectionInStringData))]
    public void ToJaggedArray_StringWithInconsistentBrackets_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_CollectionInStringInterpreter(stringWithBrackets, int.Parse).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(CollectionInStringData.Erroneous), MemberType = typeof(CollectionInStringData))]
    public void ToEnumerable_UnsupportedString_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_CollectionInStringInterpreter(stringWithBrackets, int.Parse).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Erroneous), MemberType = typeof(CollectionInStringData))]
    public void ToArray_UnsupportedString_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_CollectionInStringInterpreter(stringWithBrackets, int.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Erroneous), MemberType = typeof(CollectionInStringData))]
    public void ToJaggedArray_UnsupportedString_Throws(string stringWithBrackets)
    {
        Assert.Throws<ArgumentException>(() => SUT_CollectionInStringInterpreter(stringWithBrackets, int.Parse).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(CollectionInStringData.Empty), MemberType = typeof(CollectionInStringData))]
    public void ToEnumerable_EmptyString_Throws(string stringWithBrackets)
    {
        Assert.Throws<InvalidOperationException>(() => SUT_CollectionInStringInterpreter(stringWithBrackets, int.Parse).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Empty), MemberType = typeof(CollectionInStringData))]
    public void ToArray_EmptyString_Throws(string stringWithBrackets)
    {
        Assert.Throws<InvalidOperationException>(() => SUT_CollectionInStringInterpreter(stringWithBrackets, int.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Empty), MemberType = typeof(CollectionInStringData))]
    public void ToJaggedArray_EmptyString_Throws(string stringWithBrackets)
    {
        Assert.Throws<InvalidOperationException>(() => SUT_CollectionInStringInterpreter(stringWithBrackets, int.Parse).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(CollectionInStringData.NonJagged), MemberType = typeof(CollectionInStringData))]
    [MemberData(nameof(CollectionInStringData.SpacedNonJagged), MemberType = typeof(CollectionInStringData))]
    public void TryInterpret_CollectionInString_ReturnsInterpretedCollection(string collectionInString, int[] interpretedCollection)
    {
        object? argument = collectionInString;

        CollectionInStringInterpreter<int>.TryInterpret(ref argument, int.Parse);

        Assert.Equal(interpretedCollection, argument);
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Jagged), MemberType = typeof(CollectionInStringData))]
    [MemberData(nameof(CollectionInStringData.SpacedJagged), MemberType = typeof(CollectionInStringData))]
    public void TryInterpret_JaggedCoolectionInString_ReturnsInterpretedCollection(string collectionInString, int[][] interpretedCollection)
    {
        object? argument = collectionInString;

        CollectionInStringInterpreter<int>.TryInterpret(ref argument, int.Parse);

        Assert.Equal(interpretedCollection, argument);
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.NonJaggedDouble), MemberType = typeof(CollectionInStringData))]
    public void TryInterpret_CollectionOfDoublesInString_ReturnsInterpretedCollection(string collectionInString, double[] interpretedCollection)
    {
        object? argument = collectionInString;

        CollectionInStringInterpreter<double>.TryInterpret(ref argument, double.Parse);

        Assert.Equal(interpretedCollection, argument);
    }



    [Theory]
    [MemberData(nameof(CollectionInStringData.NonJagged), MemberType = typeof(CollectionInStringData))]
    [MemberData(nameof(CollectionInStringData.SpacedNonJagged), MemberType = typeof(CollectionInStringData))]
    public void ToEnumerable_CollectionInString_ReturnsInterpretedCollection(string collectionInString, int[] interpretedCollection)
    {
        Assert.Equal(interpretedCollection, SUT_CollectionInStringInterpreter(collectionInString, int.Parse).ToEnumerable());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.NonJagged), MemberType = typeof(CollectionInStringData))]
    [MemberData(nameof(CollectionInStringData.SpacedNonJagged), MemberType = typeof(CollectionInStringData))]
    public void ToArray_CollectionInString_ReturnsInterpretedCollection(string collectionInString, int[] interpretedCollection)
    {
        Assert.Equal(interpretedCollection, SUT_CollectionInStringInterpreter(collectionInString, int.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.Jagged), MemberType = typeof(CollectionInStringData))]
    [MemberData(nameof(CollectionInStringData.SpacedJagged), MemberType = typeof(CollectionInStringData))]
    public void ToJaggedArray_CollectionInString_ReturnsInterpretedCollection(string collectionInString, int[][] interpretedCollection)
    {
        Assert.Equal(interpretedCollection, SUT_CollectionInStringInterpreter(collectionInString, int.Parse).ToJaggedArray());
    }



    [Theory]
    [MemberData(nameof(CollectionInStringData.NonJaggedDouble), MemberType = typeof(CollectionInStringData))]
    public void ToArray_CollectionOfDoublesInString_ReturnsInterpretedCollection(string collectionInString, double[] interpretedCollection)
    {
        Assert.Equal(interpretedCollection, SUT_CollectionInStringInterpreter(collectionInString, double.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.NonJaggedChar), MemberType = typeof(CollectionInStringData))]
    public void ToArray_CollectionOfCharsInString_ReturnsInterpretedCollection(string collectionInString, char[] interpretedCollection)
    {
        Assert.Equal(interpretedCollection, SUT_CollectionInStringInterpreter(collectionInString, char.Parse).ToArray());
    }

    [Theory]
    [MemberData(nameof(CollectionInStringData.JaggedChar), MemberType = typeof(CollectionInStringData))]
    public void ToJaggedArray_JaggedCollectionOfCharsInString_ReturnsInterpretedCollection(string collectionInString, char[][] interpretedCollection)
    {
        Assert.Equal(interpretedCollection, SUT_CollectionInStringInterpreter(collectionInString, char.Parse).ToJaggedArray());
    }

}
