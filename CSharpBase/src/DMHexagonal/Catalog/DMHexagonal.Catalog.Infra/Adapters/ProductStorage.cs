using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.Base.Abstractions;
using DMHexagonal.Catalog.Core.Entries;
using DMHexagonal.Catalog.Core.ProductAggregate;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;

namespace DMHexagonal.Catalog.Infra.Adapters;

public class ProductStorage(IDispatcher dispatcher) : IProductStorage
{
    private Dictionary<ProductId, ProductEntry> _entries = new();
    private int _lastId;

    public ProductId NextIdentity() => new(++_lastId);

    public IEnumerable<ProductItem> All()
    {
        foreach (var entry in _entries.Values)
        {
            yield return ProductItem.Restore(entry);
        }
    }

    public ProductItem Load(ProductId id)
    {
        var entry = _entries.GetValueOrDefault(id);
        if (entry == null) throw new ApplicationException("Не удалось найти сущность с идентификатором " + id.Id);

        var questionnaire = ProductItem.Restore(entry);

        return questionnaire;
    }

    public void Save(ProductItem aggregate)
    {
        var entry = aggregate.Entry();

        _entries[aggregate.Id] = entry;

        var events = aggregate.Changes();
        dispatcher.Publish(events);
    }

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}