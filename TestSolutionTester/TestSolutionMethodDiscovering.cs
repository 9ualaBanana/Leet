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
        new object[] { SolutionContainerProvider.GetConstructor<SolutionProvider>() },
        new object[] { SolutionContainerProvider.GetConstructor<ResultProvider>() }
    };

    [Fact]
    public void ShouldThrowEntryPointNotFoundException_WhenNoSolutionMethodsWereDiscovered()
    {
        Assert.Throws<EntryPointNotFoundException>(SolutionContainerProvider.GetConstructor<EmptySolutionContainer>());
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenMultipleSolutionMethodsWereDiscovered()
    {
        Assert.Throws<AmbiguousMatchException>(SolutionContainerProvider.GetConstructor<MultipleSolutionMethodsProvider>());
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenBothAttributeApplied()
    {
        Assert.Throws<AmbiguousMatchException>(SolutionContainerProvider.GetConstructor<SolutionAndResultProvider>());
    }

    [Fact]
    public void ShouldThrowAmbiguousMatchException_WhenMultipleResultAttributesApplied()
    {
        Assert.Throws<AmbiguousMatchException>(SolutionContainerProvider.GetConstructor<MultipleAttributesResultProvider>());
    }

    [Fact]
    public void ShouldThrowFormatException_WhenSolutionProviderReturnsVoid()
    {
        Assert.Throws<FormatException>(SolutionContainerProvider.GetConstructor<WrongTypeSolutionProvider>());
    }

    [Fact]
    public void ShouldThrowFormatException_WhenResultProviderDoesNotReturnVoid()
    {
        Assert.Throws<FormatException>(SolutionContainerProvider.GetConstructor<WrongTypeResultProvider>());
    }
}
