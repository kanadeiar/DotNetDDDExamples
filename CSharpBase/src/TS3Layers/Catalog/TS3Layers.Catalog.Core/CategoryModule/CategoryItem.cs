using Kanadeiar.Common.Functionals;
using TS3Layers.Catalog.DataAccess.Entries;

namespace TS3Layers.Catalog.Core.CategoryModule;

public class CategoryItem(int id, string name)
{
    private readonly int _id = id
        .Require(id != 0, () => throw new ApplicationException("Идентификатор должен быть задан"));
    private readonly string _name = name
        .Require(name!.Length is >= 3 and <= 100, () => throw new ApplicationException("Название должно быть длинной от 3 до 100 символов"));

    public static CategoryItem Restore(CategoryEntry entry)
    {
        return new CategoryItem(entry.Id, entry.Name);
    }



    public CategoryEntry Entry() =>
        new()
        {
            Id = id,
            Name = _name,
        };

    public override string ToString() => $"{_name}";
}