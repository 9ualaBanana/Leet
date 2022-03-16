using CCEasy.Services.StringInterpreter;
using CCEasy.Test.Framework.TestData;
using System;
using Xunit;

namespace CCEasy.Test.Tests.Units;

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
}
