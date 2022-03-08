using System;

namespace CCHelper.Test.Framework;

internal static class AssertExtensions
{
    internal static bool DoesNotThrow(this Action action)
    {
        bool exceptionWasNotThrown = true;
        try
        {
            action();
        }
        catch (Exception)
        {
            exceptionWasNotThrown = false;
        }
        return exceptionWasNotThrown;
    }
}
