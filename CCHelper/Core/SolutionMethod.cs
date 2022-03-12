using CCHelper.Services;
using CCHelper.Services.ArgumentsProcessing;
using System.Reflection;

namespace CCHelper.Core;

/// <summary>
/// The base class that is inherited by concrete solution method implementations.
/// </summary>
/// <typeparam name="TResult">the type of the solution result.</typeparam>
internal abstract class SolutionMethod<TResult>
{
    readonly object _solutionContainer;
    object?[]? _arguments;

    readonly protected MethodInfo _method;
    protected object?[]? Arguments
    {
        get => _arguments;
        set => _arguments = new ArgumentsProcessor(_method, value).Process();
    }
    protected abstract Type ResultType { get; }

    protected SolutionMethod(MethodInfo method, object solutionContainer)
    {
        _solutionContainer = solutionContainer;
        _method = method;
    }
    /// <summary>
    /// There is no constraint that satisfies both reference and nullable types. 
    /// Checks at runtime that <typeparamref name="TResult"/> conforms to these constraints.
    /// </summary>
    /// <remarks>
    /// Must be called at the end of the child constructor.
    /// </remarks>
    /// <exception cref="ArgumentException"></exception>
    protected void EnsureResultsTypesCompatibility()
    {
        if (!TypeBinder.CanBind(ResultType, typeof(TResult)))
        {
            throw new ArgumentException($"The actual type of the solution method's result <{ResultType}> is not compatible with the type provided by the user <{typeof(TResult)}>.");
        }
    }

    internal TResult? Invoke(object?[]? arguments)
    {
        Arguments = arguments;
        var methodInfoResult = _method.Invoke(_solutionContainer, Arguments);

        // EnsureResultsTypesCompatibility() justifies the usage of the null-forgiving operator.
        return (TResult)RetrieveSolutionMethodSpecificResult(methodInfoResult)!;
    }

    protected abstract object? RetrieveSolutionMethodSpecificResult(object? methodInfoResult);
}
