using TS3Layers.Catalog.DataAccess.Entries;

namespace TS3Layers.Catalog.DataAccess.Data;

public class ProductStorage
{
    private readonly Dictionary<int, ProductEntry> _entries = new();
    private int _lastId;

    public IEnumerable<ProductEntry> Load(Predicate<ProductEntry> predicate) => _entries.Values.Where(e => predicate(e));

    public ProductEntry? Load(int id) => _entries.GetValueOrDefault(id);

    public void Save(ProductEntry entry)
    {
        if (entry.Id == 0) entry.Id = ++_lastId;

        _entries[entry.Id] = entry;
    }

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}
