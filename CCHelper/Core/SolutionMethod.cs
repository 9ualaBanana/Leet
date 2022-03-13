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
    readonly protected MethodInfo _method;

    protected object?[]? Arguments { get; private set; }
    protected abstract Type ResultType { get; }

    /// <summary>
    /// Contains all the necessary logic for ensuring the proper construction of derived classes.<br/>
    /// The constructors of concrete <see cref="SolutionMethod{TResult}"/> implementations must be used 
    /// only for calling this base constructor.<br/>
    /// The actual specific construction logic must be defined inside <see cref="Init"/>.
    /// </summary>
    /// <param name="method">the actual method definition.</param>
    /// <param name="solutionContainer">the instance of the type in which the <paramref name="method"/> is defined.</param>
    protected SolutionMethod(MethodInfo method, object solutionContainer)
    {
        _solutionContainer = solutionContainer;
        _method = method;
        Init();
        EnsureResultTypeCompatibility();
    }

    /// <summary>
    /// Derived classes must provide their construction logic in this method instead of the actual constructor.
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// The constraint that satisfies both reference and nullable types doesn't exist.<br/> 
    /// This method checks at runtime that <typeparamref name="TResult"/> conforms to these constraints.
    /// </summary>
    /// <remarks>
    /// Must be called at the end of the construction process.
    /// </remarks>
    /// <exception cref="ArgumentException"></exception>
    protected void EnsureResultTypeCompatibility()
    {
        if (!TypeBinder.CanBind(ResultType, typeof(TResult)))
        {
            throw new ArgumentException($"The actual type of the solution result <{ResultType}> " +
                $"is not compatible with the provided type parameter <{typeof(TResult)}>.");
        }
    }

    internal TResult? Invoke<TInterpreted>(object?[]? arguments, Func<string, TInterpreted> interpreter)
    {
        Arguments = new ArgumentsProcessor<TInterpreted>(_method, arguments, interpreter).Process();
        var methodInfoReturnValue = _method.Invoke(_solutionContainer, Arguments);

        // EnsureResultTypeCompatibility() justifies the usage of the null-forgiving operator.
        return (TResult)RetrieveSolutionResult(methodInfoReturnValue)!;
    }

    protected abstract object? RetrieveSolutionResult(object? methodInfoResult);
}
