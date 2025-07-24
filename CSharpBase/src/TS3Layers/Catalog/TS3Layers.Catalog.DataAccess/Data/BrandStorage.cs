using TS3Layers.Catalog.DataAccess.Entries;

namespace TS3Layers.Catalog.DataAccess.Data;

public class BrandStorage
{
    private readonly Dictionary<int, BrandEntry> _entries = new();
    private int _lastId;

    public int NextIdentity() => ++_lastId;
    
    public IEnumerable<BrandEntry> All(bool withDeleted = false) => _entries.Values.Where(e => withDeleted || !e.IsDeleted);

    public BrandEntry? Load(int id) => _entries.GetValueOrDefault(id);

    public void Save(BrandEntry entry) => _entries[entry.Id] = entry;

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}
