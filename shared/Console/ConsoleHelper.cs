using Kanadeiar.Common.Abstractions;
using Kanadeiar.Common.Wrappers;
using System.Globalization;
using System.Numerics;

namespace Kanadeiar.Common;

public class ConsoleHelper
{
    internal static IConsole console = new ConsoleWrapper();

    public static void PrintHeader(string text = "Название и описание задачи", string title = "RearmToDDD.NET")
    {
        setupConsole(title);
        outputHeaderToConsole(text);
    }

    private static void setupConsole(string title)
    {
        console.Title = title;
        console.WindowWidth = 120;
    }

    private static void outputHeaderToConsole(string text)
    {
        console.WriteLine($"┌─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┐");
        console.WriteLine($"│{text,-117}│");
        console.WriteLine($"└─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┘");

        console.WriteLine("");
    }

    public static void PrintFooter(string text = "Для завершения работы приложения нажмите любую кнопку ...")
    {
        console.WriteLine("\n" + text);
        console.ReadKey();
    }

    public static void Pause(string text = "Нажмите любую кнопку для продолжения ...")
    {
        console.WriteLine("\n" + text);
        console.ReadKey();
    }

    public static void Print(string message)
    {
        console.Write(message);
    }

    public static void PrintLine(string message)
    {
        console.WriteLine(message);
    }

    public static void PrintValueWithMessage(string message, string value)
    {
        console.WriteLine("");
        console.WriteLine(message + ":");
        console.WriteLine(value);
    }

    public static void PositionPrint(string message, int x, int y)
    {
        console.SetCursorPosition(x, y);
        console.WriteLine(message);
    }

    public static string? ReadLineFromConsole(string message)
    {
        console.Write($"{message}:>");
        return console.ReadLine();
    }

    public static T ReadNumberFromConsole<T>(string message)
        where T : INumber<T>
    {
        while (true)
        {
            console.Write($"{message}:>");
            var ci = new CultureInfo("ru-ru");
            if (T.TryParse(console.ReadLine(), ci, out T? number))
            {
                return number;
            }
            console.WriteLine("Ошибка! Введен неверный формат числа!");

            console.Beep();
        }
    }
}