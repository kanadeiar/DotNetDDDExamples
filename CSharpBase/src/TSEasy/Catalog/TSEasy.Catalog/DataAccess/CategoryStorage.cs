using TSEasy.Catalog.DataAccess.Entries;

namespace TSEasy.Catalog.DataAccess;

public class CategoryStorage
{
    private readonly Dictionary<int, CategoryEntry> _entries = new();
    private int _lastId;

    public IEnumerable<CategoryEntry> Load(Predicate<CategoryEntry> predicate) => _entries.Values.Where(e => predicate(e));

    public CategoryEntry? Load(int id) => _entries.GetValueOrDefault(id);

    public void Save(CategoryEntry entry)
    {
        if (entry.Id == 0) entry.Id = ++_lastId;

        _entries[entry.Id] = entry;
    }

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}