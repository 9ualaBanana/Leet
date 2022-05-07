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
