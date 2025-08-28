using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.Base.Abstractions;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;

namespace DMHexagonal.Catalog.Infra.Adapters;

public class BrandStorage(IDispatcher dispatcher) : IBrandStorage
{
    private Dictionary<BrandId, BrandEntry> _entries = new();
    private int _lastId;

    public BrandId NextIdentity() => new(++_lastId);

    public IEnumerable<BrandItem> All()
    {
        foreach (var entry in _entries.Values)
        {
            yield return BrandItem.Restore(entry);
        }
    }

    public BrandItem Load(BrandId id)
    {
        var entry = _entries.GetValueOrDefault(id);
        if (entry == null) throw new ApplicationException("Не удалось найти сущность с идентификатором " + id.Id);

        var questionnaire = BrandItem.Restore(entry);

        return questionnaire;
    }

    public void Save(BrandItem aggregate)
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