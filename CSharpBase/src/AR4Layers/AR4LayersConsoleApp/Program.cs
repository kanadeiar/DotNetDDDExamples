using AR4Layers.Catalog.Presentation.Scripts;
using Kanadeiar.Common;
using Kanadeiar.Common.Functionals;

ConsoleHelper.PrintHeader("Образец универсального поддомена на языке C#.", "Предметно-ориентированное проектирование на платформе .NET. Примеры приложений.");
ConsoleHelper.PrintLine("Образец: активная запись, четырехслойная архитектура и ромб тестирования.");

var generalScript = new CommonScript();
var brandsScript = new BrandScript();
var categoriesScript = new CategoryScript();

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
var productScript = new ProductScript();

var id = productScript.AddItem("newItem", 1, brands.First().Entry().Id, categories.First().Entry().Id)
    .TryGetValue(fail => throw new ApplicationException(fail.Error));
productScript.ChangeName(id, "Changed name")
    .Throw(fail => throw new ApplicationException(fail.Error));
productScript.ChangePrice(id, 44)
    .Throw(fail => throw new ApplicationException(fail.Error));

ConsoleHelper.PrintLine("Все элементы после изменений:");
items = generalScript.AllProducts()
    .TryGetValue(fail => throw new ApplicationException(fail.Error)).ToArray();
Array.ForEach(items, ConsoleHelper.PrintLine);

ConsoleHelper.PrintFooter();
