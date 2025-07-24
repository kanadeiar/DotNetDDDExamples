using TS3Layers.Catalog.DataAccess.Entries;

namespace TS3Layers.Catalog.DataAccess.Data;

public class ProductStorage
{
    private readonly Dictionary<int, ProductEntry> _entries = new();
    private int _lastId;

    public int NextIdentity() => ++_lastId;

    public IEnumerable<ProductEntry> All(bool withDeleted = false) => _entries.Values.Where(e => withDeleted || !e.IsDeleted);

    public ProductEntry? Load(int id) => _entries.GetValueOrDefault(id);

    public void Save(ProductEntry entry) => _entries[entry.Id] = entry;

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}
