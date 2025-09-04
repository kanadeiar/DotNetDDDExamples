using DMHexagonalConsoleApp.Helpers;
using Kanadeiar.Common;
using Kanadeiar.Common.Functionals;

ConsoleHelper.PrintHeader("Образец основного поддомена на языке C#.", "Предметно-ориентированное проектирование на платформе .NET. Примеры приложений.");
ConsoleHelper.PrintLine("Образец: модель предметной области, гексагональная архитектура и пирамида тестирования.");

var script = MasterHelper.CreateCommonScript();
var brandsScript = MasterHelper.CreateBrandScript();
var categoriesScript = MasterHelper.CreateCategoryScript();
var productScript = MasterHelper.CreateProductScript();

script.InitDemo()
    .Throw(fail => throw new ApplicationException(fail.Error));

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

ConsoleHelper.Pause("Нажать для начала изменений ...");

var id = productScript.AddItem("newItem", 1, brands.First().Entry().Id, categories.First().Entry().Id)
    .TryGetValue(fail => throw new ApplicationException(fail.Error));
productScript.ChangeName(id, "Changed name")
    .Throw(fail => throw new ApplicationException(fail.Error));
productScript.ChangePrice(id, 44)
    .Throw(fail => throw new ApplicationException(fail.Error));

ConsoleHelper.Pause();

ConsoleHelper.PrintLine("Все элементы после изменений:");
items = script.AllProducts()
    .TryGetValue(fail => throw new ApplicationException(fail.Error));
foreach (var text in items)
{
    ConsoleHelper.PrintLine(text);
}

ConsoleHelper.PrintFooter();
