using System.Diagnostics;

namespace Kanadeiar.Common.Functionals;

public static class Contracts
{
    [DebuggerHidden]
    public static T Require<T>(this T value, bool check)
    {
        if (check == false) throw new ArgumentException($"Контроль корректности аргумента не пройден. Тип: " + nameof(T));

        return value;
    }

    [DebuggerHidden]
    public static T RequireNot<T>(this T value, bool check)
    {
        if (check) throw new ArgumentException($"Контроль корректности аргумента не пройден. Тип: " + nameof(T));

        return value;
    }

    [DebuggerHidden]
    public static T Require<T>(this T value, bool check, Action exceptionFunc)
    {
        if (check == false) exceptionFunc();

        return value;
    }

    [DebuggerHidden]
    public static T RequireNot<T>(this T value, bool check, Action exceptionFunc)
    {
        if (check) exceptionFunc();

        return value;
    }
}