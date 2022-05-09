# Leet
*Tackling Coding Challenges as easy as it gets.*

## 👋🏼 Welcome!
Anyone who's ever worked in code editors like the one provided by LeetCode knows that it's quite a challenge in itself. **No debugger** or **IntelliSense** support make it completely *useless* except for auto-testing your solution with necessary inputs.
In case of a bug that'll inevitably crop up in your solution, you'll be forced to switch to an actual IDE anyway.

But it's *arguably* a more convenient option because now you are faced with other issues:
* *constructing necessary objects*
* *defining variables to store results*
* *validating the results*
* *feeding the input data...*

...especially **arrays** that are ubiquitous in those challenges, and only their construction alone takes a significant amount of time.
And what if they are jagged?.. \**sigh*\*

***Lit*** is designed to make working on solutions in your favorite IDE **trivial** avoiding all the common inconveniences related to that.

## 🥗 Recipes
### 🟢 Let It Help You
```c#
using Leet;//⬅️Include the package after it was installed

// Specify the result type of your solution and the type where it's defined.
var solution = new Solution<MySolution, int>();

// Test it.
solution.Test(expected: 0,
    100,
    "[1, -2, +3, -4, 5]",   // Array arguments can be provided as strings.
    "{{ 1, 2 }, { 3, 4 }}", // Jagged arrays are also supported.
    new int[] { 1, 2, 3 }   // Sure you can do that, but why complicate things?
    );

// Write your solution inside the class of your choosing.
public class MySolution
{
    [Solution]//⬅️Label your solution method
    public int ReturnResult(
           //↖️Result type
        int param,
        int[] canBeProvidedAsString,
        int[][] evenJaggedArrays,
        int[] andAsRegularArrayToo
        )
    {
        int result = default;
        return result;//⬅️Result is returned
    }
}
```

### 🔤 Store Result in Parameter
```c#
var solution = new Solution<MySolution, double[]>();

public class MySolution
{
          //↙️No attribute here
            //↙️Return type is void
    public void StoreResultInParameter(
            //↙️Label the parameter that'll store the result.
        [Result]double[] resultParameter
        )         //↖️Result type
    {
        // Modify the array in-place.
        Array.Sort(resultParameter);
        // resultParameter will reflect the changes. No need to return any value.
    }
}
```

### 📡 Interpret Them Your Way
```c#
var solution = new Solution<MySolution, double[]>();

// You can provide a custom interpreter for the elements inside a string array.
Func<string, double> doubleInterpreter = double.Parse;  // (default is int.Parse)

// Pass it as the function parameter...↘️
solution.Test("[1.5, 2.3]", interpreter: doubleInterpreter, "[[1.0], [2.0]]");

// ...or use it separately.
// Just include `using CCEasy.Services.StringInterpreter;` at the top of the file.

// Using instance interface.
string doubleArrayString = "[.1, .2, .3]";
var interpreter = new CollectionInStringInterpreter<double>(doubleArrayString, double.Parse);
double[] array = interpreter.ToArray();
var enumerable = interpreter.ToEnumerable();

string jaggedArrayString = "[ [1], [2], [3] ]";
var jaggedInterpreter = new CollectionInStringInterpreter<double>(doubleArrayString, double.Parse);
var jaggedArray = jaggedInterpreter.ToJaggedArray();

// Using static interface.
object? doubleArrayHolder = doubleArrayString;
object? jaggedArrayHolder = jaggedArrayString;
CollectionInStringInterpreter<double>.TryInterpret(ref doubleArrayHolder, double.Parse);
CollectionInStringInterpreter<int>.TryInterpret(ref jaggedArrayHolder, int.Parse);
```

## ⚙️ Features
* **Intuitive** interface
* **Arrays** can be handled as strings
* **Any** types and values are supported
* **Documentation** is included in the package
* **Visualization** support for enumerables
* **Seamless** workflow
* **Type-safe**
* **Friendly** exception messages

## 💡 Suggestions
Feel free to leave your proposals in [Issues](https://github.com/GualaBanana/Leet/issues) section. Any feedback is highly appreciated.