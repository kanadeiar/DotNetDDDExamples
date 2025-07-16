using System.Diagnostics;

namespace Kanadeiar.Common.Functionals;

public interface IFail
{
    public string Error { get; }
}

public interface IOk<out T>
{
    public T Value { get; }
}

public class Result<T>(T value) : IOk<T>
{
    T IOk<T>.Value => value;
}

public class Fail<T>(string error) : Result<T>(default!), IFail
{
    string IFail.Error => error;
}

public class Result() : Result<bool>(false)
{
    public static Result<T> Ok<T>(T value) => new(value);

    public static Result<T> Fail<T>(string error = default!) => new Fail<T>(error);

    public static Result Ok() => new();

    public static Result Fail(string error) => new Fail(error);
}

public class Fail(string error) : Result, IFail
{
    string IFail.Error => error;
}

public static class ResultSupport
{
    public static T TryGetValue<T>(this Result<T> result, Func<IFail, T> failFunc)
    {
        return result switch
        {
            IFail fail => failFunc(fail),
            IOk<T> ok => ok.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    public static T TryGetValue<T>(this Result<T> result, Func<T> failFunc)
    {
        return result switch
        {
            IFail _ => failFunc(),
            IOk<T> ok => ok.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [DebuggerHidden]
    public static T Throw<T>(this Result<T> result, Func<IFail, Exception> exceptionFunc)
    {
        return result switch
        {
            IFail fail => throw exceptionFunc(fail),
            IOk<T> ok => ok.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [DebuggerHidden]
    public static T Throw<T>(this Result<T> result, Func<Exception> exceptionFunc)
    {
        return result switch
        {
            IFail _ => throw exceptionFunc(),
            IOk<T> ok => ok.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }
}