using AR4Layers.Catalog.DataAccess;
using AR4Layers.Catalog.DataAccess.Entries;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Core.ProductModule;

public class ProductItem(int id, string name, decimal price, int brandId, int catalogId, bool isDeleted = false)
{
    private readonly int _id = id
        .Require(id != 0, () => throw new ApplicationException("Идентификатор должен быть задан"));
    private string _name = name
        .Require(name!.Length is >= 3 and <= 290, () => throw new ApplicationException("Название товара должно быть приемлемой длинны"));
    private decimal _price = price
        .Require(price is >= 0 and <= 10000, () => throw new ApplicationException("Цена товара должна быть установлена от 0 до 10000"));
    private readonly int _brandId = brandId
        .Require(id != 0, () => throw new ApplicationException("Идентификатор бренда должен быть задан"));
    private readonly int _catalogId = catalogId
        .Require(id != 0, () => throw new ApplicationException("Идентификатор каталога должен быть задан"));
    private bool _isDeleted = isDeleted;

    public static ProductItem Create(string name, decimal price, int brandId, int categoryId)
    {
        var id = Registry.ProductStorage.NextIdentity();

        return new ProductItem(id, name, price, brandId, categoryId);
    }

    public static ProductItem Restore(ProductEntry entry)
    {
        return new ProductItem(entry.Id, entry.Name, entry.Price, entry.BrandId, entry.CategoryId, entry.IsDeleted);
    }





    public ProductEntry Entry() =>
        new()
        {
            Id = id,
            Name = _name,
            Price = _price,
            BrandId = _brandId,
            CategoryId = _catalogId,
            IsDeleted = _isDeleted,
        };

    public override string ToString() => $"{_name}";

    public string FullText() => $"{_name} - {_price} руб.";
}