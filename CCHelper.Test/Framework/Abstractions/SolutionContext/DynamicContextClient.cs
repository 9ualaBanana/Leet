namespace CCHelper.Test.Framework.Abstractions.SolutionContext;

public abstract class DynamicContextClient
{
    protected readonly SolutionContextProvider _context = new();
}
