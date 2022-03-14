namespace CCEasy.Test.Framework.Abstractions.SolutionContext;

public abstract class DynamicContextFixture
{
    protected readonly SolutionContextProvider _context = new();
}
