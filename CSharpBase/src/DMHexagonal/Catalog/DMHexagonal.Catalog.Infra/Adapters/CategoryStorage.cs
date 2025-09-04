using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.Base.Abstractions;
using DMHexagonal.Catalog.Core.CategoryAggregate;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;

namespace DMHexagonal.Catalog.Infra.Adapters;

public class CategoryStorage(IDispatcher dispatcher) : ICategoryStorage
{
    private readonly Dictionary<int, CategoryEntry> _entries = new();
    private int _lastId;

    public IEnumerable<CategoryItem> Load(Predicate<CategoryEntry> predicate)
    {
        foreach (var entry in _entries.Values.Where(e => predicate(e)))
        {
            yield return CategoryItem.Restore(entry);
        }
    }

    public CategoryItem Load(CategoryId id)
    {
        var entry = _entries.GetValueOrDefault(id.Value);
        if (entry == null) throw new ApplicationException("Не удалось найти сущность с идентификатором " + id.Value);

        var questionnaire = CategoryItem.Restore(entry);

        return questionnaire;
    }

    public CategoryId Save(CategoryItem aggregate)
    {
        var entry = aggregate.Entry();

        if (entry.Id == 0) entry.Id = ++_lastId;

        _entries[entry.Id] = entry;

        var events = aggregate.Changes();
        dispatcher.Publish(events);

        return new CategoryId(entry.Id);
    }

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}