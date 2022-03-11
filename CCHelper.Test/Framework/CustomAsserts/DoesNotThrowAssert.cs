using CCHelper.Test.Framework.CustomAsserts;
using System;

namespace Xunit;

public partial class Assert
{
    /// <summary>
    /// Verifies that the action does not throw any exceptions.
    /// </summary>
    /// <param name="action">The action to be tested</param>
    public static void DoesNotThrow(Action action)
    {
        DoesNotThrow(action, null);
    }

    /// <summary>
    /// <inheritdoc cref="DoesNotThrow(Action)"/>
    /// </summary>
    /// <param name="action"><inheritdoc cref="DoesNotThrow(Action)"/></param>
    /// <param name="userMessage">The message to show when the action throws</param>
    /// <exception cref="DoesNotThrowException"></exception>
    public static void DoesNotThrow(Action action, string? userMessage)
    {
        try
        {
            action();
        }
        catch (Exception unexpectedException)
        {
            throw new DoesNotThrowException(unexpectedException.GetType(), userMessage);
        }
    }
}
