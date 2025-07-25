using Kanadeiar.Common;
using Kanadeiar.Common.Functionals;
using TS3LayersConsoleApp.Helpers;

ConsoleHelper.PrintHeader("Образец основного поддомена на языке C#.", "Предметно-ориентированное проектирование на платформе .NET. Примеры приложений.");
ConsoleHelper.PrintLine("Образец: транзакционный сценарий, трехслойная архитектура и перевернутая пирамида тестирования.");

var script = GeneralApplicationHelper.CreateGeneralScript();
var brandsScript = GeneralApplicationHelper.CreateBrandScript();
var categoriesScript = GeneralApplicationHelper.CreateCategoryScript();

script.InitDemo()
   .Throw(fail => throw new ApplicationException(fail.Error));

ConsoleHelper.Pause();

ConsoleHelper.PrintLine("Все элементы:");
var items = script.AllProducts()
    .TryGetValue(fail => throw new ApplicationException(fail.Error));
foreach (var text in items)
{
    ConsoleHelper.PrintLine(text);
}

ConsoleHelper.PrintLine("Все бренды:");
var brands = brandsScript.AllItems()
    .TryGetValue(fail => throw new ApplicationException(fail.Error));
foreach (var text in brands)
{
    ConsoleHelper.PrintLine(text.ToString());
}

ConsoleHelper.PrintLine("Все категории:");
var categories = categoriesScript.AllItems()
    .TryGetValue(fail => throw new ApplicationException(fail.Error));
foreach (var text in categories)
{
    ConsoleHelper.PrintLine(text.ToString());
}



ConsoleHelper.PrintFooter();
