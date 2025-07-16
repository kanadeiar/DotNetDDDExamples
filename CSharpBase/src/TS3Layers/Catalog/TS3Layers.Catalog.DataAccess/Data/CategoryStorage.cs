using TS3Layers.Catalog.DataAccess.Entries;

namespace TS3Layers.Catalog.DataAccess.Data;

public class CategoryStorage
{
    private readonly Dictionary<int, CategoryEntry> _entries = new();
    private int _lastId;

    public int NextIdentity() => ++_lastId;
    
    public IEnumerable<CategoryEntry> All() => _entries.Values;

    public CategoryEntry? Load(int id) => _entries.GetValueOrDefault(id);

    public void Save(CategoryEntry entry) => _entries[entry.Id] = entry;

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}
