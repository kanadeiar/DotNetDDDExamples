using DMHexagonal.Catalog.Core.Base;
using DMHexagonal.Catalog.Core.CategoryAggregate.Events;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;

namespace DMHexagonal.Catalog.Core.CategoryAggregate;

public class CategoryItem(CategoryId id, CategoryNameValue name, bool isDeleted = false) : AggregateRoot
{
    private CategoryNameValue _name = name;
    private bool _isDeleted = isDeleted;

    public override CategoryId Id => id;

    public static CategoryItem Create(CategoryId id, CategoryNameValue name)
    {
        var result = new CategoryItem(id, name);
        result.ApplyChange(new CategoryCreated(id, name));
        return result;
    }

    public static CategoryItem Restore(CategoryEntry entry)
    {
        return new CategoryItem(new CategoryId(entry.Id), new CategoryNameValue(entry.Name), entry.IsDeleted);
    }
    
    public void Rename(CategoryNameValue newName)
    {
        _name = newName;
        ApplyChange(new CategoryRenamed(Id, newName));
    }

    public void Delete()
    {
        _isDeleted = true;
        ApplyChange(new CategoryDeleted(Id));
    }

    public CategoryEntry Entry() =>
        new()
        {
            Id = Id.Id,
            Name = _name.Name,
            IsDeleted = _isDeleted,
        };

    public override string ToString() => $"{_name.Name}";
}