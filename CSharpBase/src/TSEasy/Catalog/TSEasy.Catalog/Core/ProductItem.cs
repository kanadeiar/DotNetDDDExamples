using Kanadeiar.Common.Functionals;
using TSEasy.Catalog.DataAccess.Entries;

namespace TSEasy.Catalog.Core;

public class ProductItem(int id, string name, decimal price, int brandId, int categoryId, bool isDeleted = false)
{
    private readonly int _id = id
        .Require(id >= 0, () => throw new ApplicationException("Идентификатор должен быть задан"));
    private string _name = name
        .Require(name!.Length is >= 3 and <= 290, () => throw new ApplicationException("Название товара должно быть приемлемой длинны"));
    private decimal _price = price
        .Require(price is >= 0 and <= 10000, () => throw new ApplicationException("Цена товара должна быть установлена от 0 до 10000"));
    private readonly int _brandId = brandId
        .Require(brandId != 0, () => throw new ApplicationException("Идентификатор бренда должен быть задан"));
    private readonly int _categoryId = categoryId
        .Require(categoryId != 0, () => throw new ApplicationException("Идентификатор каталога должен быть задан"));
    private bool _isDeleted = isDeleted;

    public static ProductItem Restore(ProductEntry entry)
    {
        return new ProductItem(entry.Id, entry.Name, entry.Price, entry.BrandId, entry.CategoryId, entry.IsDeleted);
    }

    public void Rename(string newName)
    {
        if (newName.Length is >= 3 and <= 300 == false) throw new ApplicationException("Новое название товара должно быть приемлемой длинны");

        _name = newName;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice is >= 0 and <= 10000 == false) throw new ApplicationException("Новая цена товара должна быть установлена от 0 до 10000");

        _price = newPrice;
    }

    public void Delete()
    {
        _isDeleted = true;
    }

    public override string ToString() => $"{_name}";

    public string FullText() => $"{_name} - {_price} руб.";

    public ProductEntry Entry() =>
        new()
        {
            Id = _id,
            Name = _name,
            Price = _price,
            BrandId = _brandId,
            CategoryId = _categoryId,
            IsDeleted = _isDeleted,
        };
}