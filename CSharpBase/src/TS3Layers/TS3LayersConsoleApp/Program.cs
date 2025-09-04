using Kanadeiar.Common;
using Kanadeiar.Common.Functionals;
using TS3LayersConsoleApp.Helpers;

ConsoleHelper.PrintHeader("Образец основного поддомена на языке C#.", "Предметно-ориентированное проектирование на платформе .NET. Примеры приложений.");
ConsoleHelper.PrintLine("Образец: транзакционный сценарий, трехслойная архитектура и перевернутая пирамида тестирования.");

var generalScript = CommonHelper.CreateGeneralScript();
var brandsScript = CommonHelper.CreateBrandScript();
var categoriesScript = CommonHelper.CreateCategoryScript();

generalScript.InitDemo()
   .Throw(fail => throw new ApplicationException(fail.Error));

ConsoleHelper.Pause();

ConsoleHelper.PrintLine("Все элементы:");
var items = generalScript.AllProducts()
    .TryGetValue(fail => throw new ApplicationException(fail.Error)).ToArray();
Array.ForEach(items, ConsoleHelper.PrintLine);

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

ConsoleHelper.Pause("Нажать для начала изменений ...");
var script = CommonHelper.CreateScript();

var id = script.AddItem("newItem", 1, brands.First().Entry().Id, categories.First().Entry().Id)
    .TryGetValue(fail => throw new ApplicationException(fail.Error));
script.ChangeName(id, "Changed name")
    .Throw(fail => throw new ApplicationException(fail.Error));
script.ChangePrice(id, 44)
    .Throw(fail => throw new ApplicationException(fail.Error));

ConsoleHelper.PrintLine("Все элементы после изменений:");
items = generalScript.AllProducts()
    .TryGetValue(fail => throw new ApplicationException(fail.Error)).ToArray();
Array.ForEach(items, ConsoleHelper.PrintLine);

ConsoleHelper.PrintFooter();
