using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.Base.Abstractions;
using DMHexagonal.Catalog.Core.Entries;
using DMHexagonal.Catalog.Core.ProductAggregate;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;

namespace DMHexagonal.Catalog.Infra.Adapters;

public class ProductStorage(IDispatcher dispatcher) : IProductStorage
{
    private readonly Dictionary<Guid, ProductEntry> _entries = new();

    public IEnumerable<ProductItem> Load(Predicate<ProductEntry> predicate)
    {
        foreach (var entry in _entries.Values.Where(e => predicate(e)))
        {
            yield return ProductItem.Restore(entry);
        }
    }

    public ProductItem Load(ProductId id)
    {
        var entry = _entries.GetValueOrDefault(id.Value);
        if (entry == null) throw new ApplicationException("Не удалось найти сущность с идентификатором " + id.Value);

        var questionnaire = ProductItem.Restore(entry);

        return questionnaire;
    }

    public ProductId Save(ProductItem aggregate)
    {
        var entry = aggregate.Entry();

        _entries[entry.Id] = entry;

        var events = aggregate.Changes();
        dispatcher.Publish(events);

        return new ProductId(entry.Id);
    }

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}