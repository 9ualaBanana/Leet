using Leet.Test.Framework.TestData;
using System.IO;
using Xunit;

namespace Leet.Test.Tests.Components;

public class TestSolution
{
    [Fact]
    public void Test_CreatesNewSolutionContainerPerTest()
    {
        var solution = new Solution<HasSingleSolutionMethod, int>();
        var resultStream = new MemoryStream(new byte[100]);

        solution.SetResultOutput(resultStream);
        solution.Test(default);
        var firstResult = resultStream.ReadByte();
        solution.Test(default);
        var secondResult = resultStream.ReadByte();

        Assert.Equal(firstResult, secondResult);
    }
}

public class SolutionWithInstanceMembers
{
    public int FreshForEachTest = 0;

    [Solution]
    public int ChangeInstanceMember()
    {
        FreshForEachTest++;
        return FreshForEachTest;
    }
}
