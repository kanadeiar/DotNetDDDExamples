using DMHexagonal.Catalog.Core.Base;
using DMHexagonal.Catalog.Core.BrandAggregate.Events;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;

namespace DMHexagonal.Catalog.Core.BrandAggregate;

public class BrandItem(BrandId id, BrandNameValue name, bool isDeleted = false) : AggregateRoot
{
    private BrandNameValue _name = name;
    private bool _isDeleted = isDeleted;

    public override BrandId Id => id;

    public static BrandItem Create(BrandId id, BrandNameValue name)
    {
        var result = new BrandItem(id, name);
        result.ApplyChange(new BrandCreated(id, name));
        return result;
    }

    public static BrandItem Restore(BrandEntry entry)
    {
        return new BrandItem(new BrandId(entry.Id), new BrandNameValue(entry.Name), entry.IsDeleted);
    }
    
    public void Rename(BrandNameValue newName)
    {
        _name = newName;
        ApplyChange(new BrandRenamed(Id, _name));
    }

    public void Delete()
    {
        _isDeleted = true;
        ApplyChange(new BrandDeleted(Id));
    }

    public BrandEntry Entry() =>
        new()
        {
            Id = Id.Id,
            Name = _name.Name,
            IsDeleted = _isDeleted,
        };

    public override string ToString() => $"{_name.Name}";
}