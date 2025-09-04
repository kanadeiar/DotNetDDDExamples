using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.Base.Abstractions;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;

namespace DMHexagonal.Catalog.Infra.Adapters;

public class BrandStorage(IDispatcher dispatcher) : IBrandStorage
{
    private readonly Dictionary<int, BrandEntry> _entries = new();
    private int _lastId;

    public IEnumerable<BrandItem> Load(Predicate<BrandEntry> predicate)
    {
        foreach (var entry in _entries.Values.Where(e => predicate(e)))
        {
            yield return BrandItem.Restore(entry);
        }
    }

    public BrandItem Load(BrandId id)
    {
        var entry = _entries.GetValueOrDefault(id.Value);
        if (entry == null) throw new ApplicationException("Не удалось найти сущность с идентификатором " + id.Value);

        var questionnaire = BrandItem.Restore(entry);

        return questionnaire;
    }

    public BrandId Save(BrandItem aggregate)
    {
        var entry = aggregate.Entry();

        if (entry.Id == 0) entry.Id = ++_lastId;

        _entries[entry.Id] = entry;

        var events = aggregate.Changes();
        dispatcher.Publish(events);

        return new BrandId(entry.Id);
    }

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}