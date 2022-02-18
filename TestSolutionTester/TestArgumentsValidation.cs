using Xunit;
using System;
using System.Collections.Generic;

namespace TestSolutionTester;

public class TestArgumentsValidation
{
    [Theory]
    [MemberData(nameof(DifferentTypeArguments))]
    public void ShouldThrowArgumentException_WhenArgumentsOfWrongTypePassed(object argument)
    {
        var solutionMethod = SolutionTesterConstructorProvider.For<InputSolution>().Invoke().Test;
        var solutionMethodParameterType = solutionMethod.Method.GetParameters()[0].ParameterType;
        if (solutionMethodParameterType == argument.GetType()) return;

        Assert.Throws<ArgumentException>(() => solutionMethod(null!, new object[] { argument }));
    }
    public static IEnumerable<object[]> DifferentTypeArguments
    {
        get
        {
            yield return new object[] { default(char) };
            yield return new object[] { default(bool) };
            yield return new object[] { default(long) };
            yield return new object[] { default(short) };
            yield return new object[] { default(double) };
            yield return new object[] { default(float) };
            yield return new object[] { string.Empty };
        }
    }

    [Fact]
    public void ShouldThrowArgumentNullException_WhenNullArgumentsProvided()
    {
        var solutionMethod = SolutionTesterConstructorProvider.For<InputSolution>().Invoke().Test;
        Assert.Throws<ArgumentNullException>(() => solutionMethod(null!, null));
        Assert.Throws<ArgumentNullException>(() => solutionMethod(null!, new object[] { null, null }));
    }
}