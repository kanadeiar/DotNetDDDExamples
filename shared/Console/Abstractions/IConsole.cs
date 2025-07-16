namespace Kanadeiar.Common.Abstractions;

public interface IConsole
{
    string Title { get; set; }
    int WindowWidth { get; set; }
    void SetCursorPosition(int x, int y);
    void Write(string message);
    void WriteLine(string message);
    string? ReadLine();
    ConsoleKeyInfo? ReadKey();
    void Beep();
}