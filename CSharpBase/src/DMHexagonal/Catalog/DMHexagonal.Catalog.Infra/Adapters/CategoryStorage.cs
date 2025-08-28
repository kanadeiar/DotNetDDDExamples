using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.Base.Abstractions;
using DMHexagonal.Catalog.Core.CategoryAggregate;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;

namespace DMHexagonal.Catalog.Infra.Adapters;

public class CategoryStorage(IDispatcher dispatcher) : ICategoryStorage
{
    private Dictionary<CategoryId, CategoryEntry> _entries = new();
    private int _lastId;

    public CategoryId NextIdentity() => new(++_lastId);

    public IEnumerable<CategoryItem> All()
    {
        foreach (var entry in _entries.Values)
        {
            yield return CategoryItem.Restore(entry);
        }
    }

    public CategoryItem Load(CategoryId id)
    {
        var entry = _entries.GetValueOrDefault(id);
        if (entry == null) throw new ApplicationException("Не удалось найти сущность с идентификатором " + id.Id);

        var questionnaire = CategoryItem.Restore(entry);

        return questionnaire;
    }

    public void Save(CategoryItem aggregate)
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