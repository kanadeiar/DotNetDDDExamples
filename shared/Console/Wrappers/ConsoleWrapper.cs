using System;
using Kanadeiar.Common.Abstractions;

namespace Kanadeiar.Common.Wrappers;

public class ConsoleWrapper : IConsole
{
    public string Title
    {
        get => Console.Title;
        set => Console.Title = value;
    }

    public int WindowWidth
    {
        get => Console.WindowWidth;
        set => Console.WindowWidth = value;
    }

    public void SetCursorPosition(int x, int y)
    {
        Console.SetCursorPosition(x, y);
    }

    public void Write(string message)
    {
        Console.Write(message);
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public ConsoleKeyInfo? ReadKey()
    {
        return Console.ReadKey();
    }

    public void Beep()
    {
        Console.Beep();
    }
}