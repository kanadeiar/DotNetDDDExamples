using Kanadeiar.Common.Functionals;
using TS3Layers.Catalog.DataAccess.Entries;

namespace TS3Layers.Catalog.Core.ProductModule;

public class ProductItem(int id, string name, int brandId, int catalogId, decimal price)
{
    private readonly int _id = id
        .Require(id != 0, () => throw new ApplicationException("Идентификатор должен быть задан"));
    private readonly string _name = name
        .Require(name!.Length is >= 3 and <= 290, () => throw new ApplicationException("Название должно быть длинной от 3 до 290 символов"));
    private readonly int _brandId = brandId
        .Require(id != 0, () => throw new ApplicationException("Идентификатор бренда должен быть задан"));
    private readonly int _catalogId = catalogId
        .Require(id != 0, () => throw new ApplicationException("Идентификатор каталога должен быть задан"));
    private readonly decimal _price = price
        .Require(price is >= 0 and <= 10000, () => throw new ApplicationException("Должна быть установлена цена от 0 до 10000"));

    public static ProductItem Restore(ProductEntry entry)
    {
        return new ProductItem(entry.Id, entry.Name, entry.BrandId, entry.CategoryId, entry.Price);
    }



    public ProductEntry Entry() =>
        new()
        {
            Id = id,
            Name = _name,
            BrandId = _brandId,
            CategoryId = _catalogId,
            Price = _price,
        };

    public override string ToString() => $"{_name}";
}