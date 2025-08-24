using Kanadeiar.Common.Functionals;
using TSEasy.Catalog.DataAccess.Entries;

namespace TSEasy.Catalog.Core;

public class BrandItem(int id, string name, bool isDeleted = false)
{
    private readonly int _id = id
        .Require(id != 0, () => throw new ApplicationException("Идентификатор должен быть задан"));
    private string _name = name
        .Require(name!.Length is >= 3 and <= 300, () => throw new ApplicationException("Название должно быть приемлемой длинны"));
    private bool _isDeleted = isDeleted;

    public static BrandItem Restore(BrandEntry entry)
    {
        return new BrandItem(entry.Id, entry.Name, entry.IsDeleted);
    }

    public void Rename(string newName)
    {
        if (newName.Length is >= 3 and <= 300 == false) throw new ApplicationException("Новое название бренда товара должно быть приемлемой длинны");

        _name = newName;
    }

    public void Delete()
    {
        _isDeleted = true;
    }

    public BrandEntry Entry() =>
        new()
        {
            Id = _id,
            Name = _name,
            IsDeleted = _isDeleted
        };

    public override string ToString() => $"{_name}";
}