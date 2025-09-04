using TS3Layers.Catalog.DataAccess.Entries;

namespace TS3Layers.Catalog.DataAccess.Data;

public class BrandStorage
{
    private readonly Dictionary<int, BrandEntry> _entries = new();
    private int _lastId;

    public IEnumerable<BrandEntry> Load(Predicate<BrandEntry> predicate) => _entries.Values.Where(e => predicate(e));

    public BrandEntry? Load(int id) => _entries.GetValueOrDefault(id);

    public void Save(BrandEntry entry)
    {
        if (entry.Id == 0) entry.Id = ++_lastId;

        _entries[entry.Id] = entry;
    }

    public void BeginTransaction() { }
    public void Commit() { }
    public void Rollback() { }
}