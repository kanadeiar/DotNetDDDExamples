using AR4Layers.Catalog.DataAccess;
using AR4Layers.Catalog.DataAccess.Entries;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Core.CategoryModule;

public class CategoryItem(int id, string name, bool isDeleted = false)
{
    private readonly int _id = id
        .Require(id != 0, () => throw new ApplicationException("Идентификатор должен быть задан"));
    private string _name = name
        .Require(name!.Length is >= 3 and <= 300, () => throw new ApplicationException("Название должно быть приемлемой длинны"));
    private bool _isDeleted = isDeleted;

    public static CategoryItem Create(string name)
    {
        var id = Registry.ProductStorage.NextIdentity();

        return new CategoryItem(id, name);
    }

    public static CategoryItem Restore(CategoryEntry entry)
    {
        return new CategoryItem(entry.Id, entry.Name, entry.IsDeleted);
    }






    public CategoryEntry Entry() =>
        new()
        {
            Id = id,
            Name = _name,
            IsDeleted = _isDeleted,
        };

    public override string ToString() => $"{_name}";
}