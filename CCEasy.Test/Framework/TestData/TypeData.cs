using System;
using System.Collections.Generic;

namespace CCEasy.Test.Framework.TestData;

public static class TypeData
{
    internal static Type DummyType = typeof(int);
    internal static int DummyValue = default;

    public static IEnumerable<object[]> ValueTypes
    {
        get
        {
            yield return new object[] { typeof(int) };
            yield return new object[] { typeof(short) };
            yield return new object[] { typeof(long) };
            yield return new object[] { typeof(float) };
            yield return new object[] { typeof(double) };
            yield return new object[] { typeof(decimal) };
            yield return new object[] { typeof(char) };
            yield return new object[] { typeof(bool) };

        }
    }
    public static IEnumerable<object[]> NullableTypes
    {
        get
        {
            yield return new object[] { typeof(int?) };
            yield return new object[] { typeof(short?) };
            yield return new object[] { typeof(long?) };
            yield return new object[] { typeof(float?) };
            yield return new object[] { typeof(double?) };
            yield return new object[] { typeof(decimal?) };
            yield return new object[] { typeof(char?) };
            yield return new object[] { typeof(bool?) };
        }
    }
    public static IEnumerable<object[]> ReferenceTypes
    {
        get
        {
            yield return new object[] { typeof(object) };
            yield return new object[] { typeof(string) };
        }
    }

    public static IEnumerable<object[]> DefaultValues
    {
        get
        {
            yield return new object[] { default(int) };
            yield return new object[] { default(char) };
            yield return new object[] { default(bool) };
            yield return new object[] { default(long) };
            yield return new object[] { default(short) };
            yield return new object[] { default(double) };
            yield return new object[] { default(float) };
            yield return new object[] { default(decimal) };
        }
    }
}
