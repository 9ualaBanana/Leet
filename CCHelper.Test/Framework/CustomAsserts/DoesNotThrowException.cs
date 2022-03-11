using System;
using Xunit.Sdk;

namespace CCHelper.Test.Framework.CustomAsserts;

/// <summary>
/// Exception thrown when code unexpectedly throws an exception.
/// </summary>
public class DoesNotThrowException : AssertActualExpectedException
{
    readonly string? stackTrace = null;

    /// <summary>
    /// Creates a new instance of the <see cref="DoesNotThrowExceptionException"/> class. Call this constructor
    /// when exception was thrown.
    /// </summary>
    /// <param name="unexpectedException">The type of the exception that wasn't expected</param>
    public DoesNotThrowException(Type unexpectedException, string? userMessage)
        : base("(No exception)", unexpectedException, userMessage!, null!, null!)
    { }

    /// <summary>
    /// Gets a string representation of the frames on the call stack at the time the current exception was thrown.
    /// </summary>
    /// <returns>A string that describes the contents of the call stack, with the most recent method call appearing first.</returns>
    public override string StackTrace => stackTrace ?? base.StackTrace;
}
