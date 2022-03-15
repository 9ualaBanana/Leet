<h1 align="center"> CCEasy </h1>
<p align="center"><em> Tackling Coding Challenge solutions in the IDE of your choice has never been easier. </em></p>

## 👋🏼 Welcome!
Anyone who's ever worked in code editors like the one provided by LeetCode knows that it's quite a challenge in itself.
**No debugger** or **IntelliSense** support make it completely **useless** except for auto-testing your solution with necessary inputs.
In case of a bug that'll inevitably crop up in your solution, you'll be forced to switch to an actual IDE *anyway*.

But it's *arguably* a more convenient option because now you are faced with **other issues**:
* *constructing necessary objects*
* *defining variables to store results*
* *validating the results*
* *providing the input data...*

...especially **arrays** that are **ubiquitous** in those challenges, and only their construction **alone** takes a **significant** amount of time.
And what if they are **jagged**?.. \**sigh*\*

***CCEasy*** is designed to make working on solutions in your favorite IDE **trivial** avoiding all the common inconveniences  related to that.

## 🥗 Recipes
### 🟢 Let It Help You
```c#
using CCEasy;//⬅️Include the package after it was installed

// Specify the result type of your solution and the type where it's defined.
var solutionTester = new SolutionTester<Solution, int>();

// Exercise it providing the expected result and arguments.
solutionTester.Test(expected: 0,
    100,
    "[1, -2, +3, -4, 5]",   // Array arguments can be provided as strings.
    "{{ 1, 2 }, { 3, 4 }}", // Jagged arrays too.
    new int[] { 1, 2, 3 }   // Sure you can do that, but why complicate things?
    );

// Write your solution inside the class of your choosing.
public class Solution
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
var solutionTester = new SolutionTester<MySolution, double[]>();

public class MySolution
{
          //↙️No attribute here
            //↙️Return type is void
    public void StoreResultInParameter(
            //↙️The attribute is applied to the parameter of interest.
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
var solutionTester = new SolutionTester<Solution, double[]>();

// You can provide a custom interpreter for the elements inside a string array.
Func<string, double> doubleInterpreter = double.Parse;  // (default is int.Parse)

// Pass it as the function parameter...↘️
solutionTester.Test("[1.5, 2.3]", interpreter: doubleInterpreter, "[[1.0], [2.0]]");

// ...or use it separately.
// Just include `using CCEasy.Services.StringInterpreter;` at the top of the file.

// Via instance interface.
string doubleArrayString = "[.1, .2, .3]";
var interpreter = new CollectionInStringInterpreter<double>(doubleArrayString, double.Parse);
double[] array = interpreter.ToArray();
var enumerable = interpreter.ToEnumerable();

string jaggedArrayString = "[ [1], [2], [3] ]";
var jaggedInterpreter = new CollectionInStringInterpreter<double>(doubleArrayString, double.Parse);
var jaggedArray = jaggedInterpreter.ToJaggedArray();

// Via static interface.
object? doubleArrayHolder = doubleArrayString;
object? jaggedArrayHolder = jaggedArrayString;
CollectionInStringInterpreter<double>.TryInterpret(ref doubleArrayHolder, double.Parse);
CollectionInStringInterpreter<int>.TryInterpret(ref jaggedArrayHolder, int.Parse);
```

### 🚰 Change Output
```c#
solutionTester.SetResultOutput(File.Open("solution_results.txt", FileMode.OpenOrCreate));
```

## ⚙️ Features
* ***Intuitive*** interface
* ***Documentation*** is included in the package
* ***Arrays*** can be handled as strings
* ***Visualization*** support for enumerables
* ***Any*** types and values are supported
* ***Seamless*** workflow
* ***Type-safe***
* ***Friendly*** exception messages

## 💿 Installation

[Download page](https://www.nuget.org/packages/CCEasy/)

For guidance on how to install NuGet packages refer 
[here](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-using-the-dotnet-cli) 
and [here](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio).

## 💡 Suggestions
Feel free to leave your proposals in [Issues](https://github.com/GualaBanana/CCEasy/issues) section. Any feedback is highly appreciated.

***
### 🍌 GualaBanana
