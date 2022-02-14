using Xunit;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace TestSolutionTester;

public class TestSolutionMethodDiscovering
{
    [Theory]
    [MemberData(nameof(ConstructorsOfContainersWithValidSolutionMethods))]
    public void ShouldNotThrow_WhenSolutionContainerDefinesValidSolutionMethod(Action constructor)
    {
        bool exceptionWasThrown = false;
        try
        {
            constructor();
        }
        catch (Exception)
        {
            exceptionWasThrown = true;
        }
        Assert.False(exceptionWasThrown);
    }
    public static IEnumerable<object[]> ConstructorsOfContainersWithValidSolutionMethods = new List<object[]>()
    {
        new object[] { SolutionContainerProvider.GetConstructor<OutputSolution>() },
        new object[] { SolutionContainerProvider.GetConstructor<InputSolution>() }
    };

    [Fact]
    public void ShouldThrowEntryPointNotFoundException_WhenNoSolutionMethodsWereDiscovered()
    {
        Assert.Throws<EntryPointNotFoundException>(SolutionContainerProvider.GetConstructor<IgnoredSolutionMethods>());
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenMultipleSolutionMethodsWereDiscovered()
    {
        Assert.Throws<AmbiguousMatchException>(SolutionContainerProvider.GetConstructor<MultipleSolutionMethods>());
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenBothAttributeApplied()
    {
        Assert.Throws<AmbiguousMatchException>(SolutionContainerProvider.GetConstructor<InputAndOutputSolutions>());
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenMultipleResultAttributesApplied()
    {
        Assert.Throws<AmbiguousMatchException>(SolutionContainerProvider.GetConstructor<MultipleInputSolutionAttributes>());
    }

    [Fact]
    public void ShouldThrowFormatException_WhenSolutionProviderReturnsVoid()
    {
        Assert.Throws<FormatException>(SolutionContainerProvider.GetConstructor<OutputSolutionWrongType>());
    }

    [Fact]
    public void ShouldThrowFormatException_WhenResultProviderDoesNotReturnVoid()
    {
        Assert.Throws<FormatException>(SolutionContainerProvider.GetConstructor<InputSolutionWrongType>());
    }
}
