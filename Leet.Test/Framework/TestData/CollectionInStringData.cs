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

using System.Collections.Generic;

namespace Leet.Test.Framework.TestData;

internal static class CollectionInStringData
{
    public static IEnumerable<object[]> NonJagged
    {
        get
        {
            yield return new object[]
            {
                "[0]", new int[] { 0 }
            };
            yield return new object[]
            {
                "[-1]", new int[] { -1 }
            };
            yield return new object[]
            {
                "[+2]", new int[] { 2 }
            };
            yield return new object[]
            {
                "[12345]", new int[] { 12345 }
            };
            yield return new object[]
            {
                "[+54321]", new int[] { 54321 }
            };
            yield return new object[]
            {
                "[-6789]", new int[] { -6789 }
            };
            yield return new object[]
            {
                "[0, 1]", new int[] { 0, 1 }
            };
            yield return new object[]
            {
                "[1, 2, 3, 4, 5]", new int[] { 1, 2, 3, 4, 5 }
            };
            yield return new object[]
            {
                "[100, 2233, 315, 489, 512]", new int[] { 100, 2233, 315, 489, 512 }
            };
            yield return new object[]
            {
                "[+456, -63, -1, +2890, +835]", new int[] { 456, -63, -1, 2890, 835 }
            };
            yield return new object[]
            {
                "[ +25]", new int[] { 25 }
            };
            yield return new object[]
            {
                " [ -1000 ]", new int[] { -1000 }
            };
            yield return new object[]
            {
                "  [  -210 ] ", new int[] { -210 }
            };
            yield return new object[]
            {
                "[ +4, -6, -1, +2, +8 ]", new int[] { 4, -6, -1, 2, 8 }
            };
            yield return new object[]
            {
                "[5, 5, +5, 5, 5]", new int[] {5, 5, 5, 5, 5 }
            };
            yield return new object[]
            {
                " [2, 2, 0, -1, -10] ", new int[] {2, 2, 0, -1, -10 }
            };
            yield return new object[]
            {
                " [ 1, 10, 30, -100, 20 ] ", new int[] {1, 10, 30, -100, 20 }
            };
        }
    }

    public static IEnumerable<object[]> Jagged
    {
        get
        {
            yield return new object[]
            {
                "[ [1, 21] [5, 6] ]", new int[][] { new int[] { 1, 21 }, new int[] { 5, 6 } }
            };
            yield return new object[]
            {
                "[ [0, -200], [45]]", new int[][] { new int[] { 0, -200 }, new int[] { 45 } }
            };
            yield return new object[]
            {
                "[[10]]", new int[][] { new int[] { 10 } }
            };
            yield return new object[]
            {
                "[ [ 10] ] ", new int[][] { new int[] { 10 } }
            };
            yield return new object[]
            {
                "[[ 98, 2, 51 ], [5]]", new int[][] { new int[] { 98, 2, 51 }, new int[] { 5 } }
            };
            yield return new object[]
            {
                "[[ 11 ], [12] , [32], [44] ] ", new int[][] { new int[] { 11 }, new int[] { 12 }, new int[] { 32 }, new int[] { 44 } }
            };
            yield return new object[]
            {
                "  [ [1, 25] , [ 3 ], [74]  ] ", new int[][] { new int[] { 1, 25 }, new int[] { 3 }, new int[] { 74 } }
            };
            yield return new object[]
            {
                "( ( 4, 3 ), (69, 5 ) )", new int[][] { new int[] { 4, 3 }, new int[] { 69, 5 } }
            };

        }
    }

    public static IEnumerable<object[]> SpacedNonJagged
    {
        get
        {
            yield return new object[]
            {
                @"[
                    1, 1, 1, 1, 0
                ]", new int[] { 1, 1, 1, 1, 0 }
            };
            yield return new object[]
            {
                "[\n 1, 1, 1, 1, 0" +
                "]", new int[] { 1, 1, 1, 1, 0 }
            };
            yield return new object[]
            {
                "[\r\n 1, 1, 1, 1, 0]", new int[] { 1, 1, 1, 1, 0 }
            };
            yield return new object[]
            {
                "[\n" +
                "   1, 1, 1, 1, 0]", new int[] { 1, 1, 1, 1, 0 }
            };
        }
    }

    public static IEnumerable<object[]> SpacedJagged
    {
        get
        {
            yield return new object[]
            {
                @"[
                    [1, 1, 1, 1, 0]
                ]", new int[][] { new int[] { 1, 1, 1, 1, 0 } }
            };
            yield return new object[]
            {
                "[\n[1, 1, 1, 1, 0]" +
                "]", new int[][] { new int[] { 1, 1, 1, 1, 0 } }
            };
            yield return new object[]
            {
                "[\r\n[1, 1, 1, 1, 0]" +
                "]", new int[][] { new int[] { 1, 1, 1, 1, 0 } }
            };
            yield return new object[]
            {
                "[\n" +
                "   [1, 1, 1, 1, 0]" +
                "]", new int[][] { new int[] { 1, 1, 1, 1, 0 } }
            };
        }
    }

    public static IEnumerable<object[]> NonJaggedDouble
    {
        get
        {
            yield return new object[]
            {
                " [ .1, +175.10, 30.0, -100.1, -.20 ] ", new double[] {.1, 175.10, 30.0, -100.1, -.20 }
            };
            yield return new object[]
            {
                "[-.4]", new double[] { -.4 }
            };
            yield return new object[]
            {
                "[+.4]", new double[] { .4 }
            };
            yield return new object[]
            {
                "[-.4, 15.0]", new double[] { -.4, 15 }
            };
            yield return new object[]
            {
                "[5.4, -15]", new double[] { 5.4, -15 }
            };
        }
    }

    public static IEnumerable<object[]> NonJaggedChar
    {
        get
        {
            yield return new object[]
            {
                " [ ., +, 3, -, 0 ] ", new char[] {'.', '+', '3', '-', '0' }
            };
            yield return new object[]
            {
                "[.]", new char[] { '.' }
            };
            yield return new object[]
            {
                "[ 4 ]", new char[] { '4' }
            };
            yield return new object[]
            {
                "[ -, 0 ]", new char[] { '-', '0' }
            };
            yield return new object[]
            {
                "  [ * , # ] ", new char[] { '*', '#' }
            };
        }
    }

    public static IEnumerable<object[]> JaggedChar
    {
        get
        {
            yield return new object[]
            {
                " [ [., +], [3, -, 0] ] ", new char[][] { new char[] { '.', '+' }, new char[] { '3', '-', '0' } }
            };
            yield return new object[]
            {
                "[[.]]", new char[][] { new char[] { '.' } }
            };
            yield return new object[]
            {
                "[ [4 ] ]", new char[][] { new char[] { '4' } }
            };
            yield return new object[]
            {
                "[[-], [0] ]", new char[][] { new char[] { '-' }, new char[] { '0' } }
            };
            yield return new object[]
            {
                "  [[ * , # ]] ", new char[][] { new char[] { '*', '#' } }
            };
        }
    }


    public static IEnumerable<object[]> Erroneous
    {
        get
        {
            yield return new object[] { "]" };
            yield return new object[] { "[" };
            yield return new object[] { "[]" };
            yield return new object[] { "[ ]" };
            yield return new object[] { "[[[[" };
            yield return new object[] { "]]]]" };
            yield return new object[] { "unsupported" };
            yield return new object[] { "(>" };
            yield return new object[] { "{]" };
            yield return new object[] { "[)" };
            yield return new object[] { "<}" };
        }
    }

    public static IEnumerable<object[]> Empty
    {
        get
        {
            yield return new object[] { string.Empty };
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { "         " };
        }
    }
}
