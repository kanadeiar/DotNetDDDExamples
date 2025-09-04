using DMHexagonal.Catalog.Core.Base.Abstractions;
using DMHexagonal.Catalog.Core.ProductAggregate.Events;

namespace DMHexagonalConsoleApp.Scripts;

public static class DeveloperScript
{
    public static void Run(IRegisterDispatcher dispatcher)
    {
        dispatcher.RegisterHandler<ProductCreated>(ev =>
        {
            Console.WriteLine($"## Событие создания нового товара c Id: {ev.Id} Имя: {ev.Name} - {ev.Price} руб.");
        });

        dispatcher.RegisterHandler<ProductRenamed>(ev =>
        {
            Console.WriteLine($"## Событие переименования товара c Id: {ev.Id} Новое имя: {ev.Name}");
        });
    }
}