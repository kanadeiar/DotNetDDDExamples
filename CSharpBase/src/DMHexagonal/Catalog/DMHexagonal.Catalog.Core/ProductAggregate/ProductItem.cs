using DMHexagonal.Catalog.Core.Base;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.CategoryAggregate.Events;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;
using DMHexagonal.Catalog.Core.ProductAggregate.Events;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;

namespace DMHexagonal.Catalog.Core.ProductAggregate;

public class ProductItem(ProductId id, ProductNameValue name, PriceValue price, BrandId brandId, CategoryId categoryId, bool isDeleted = false) : AggregateRoot
{
    private ProductNameValue _name = name;
    private PriceValue _price = price;
    private BrandId _brandId = brandId;
    private CategoryId _categoryId = categoryId;
    private bool _isDeleted = isDeleted;

    public override ProductId Id => id;

    public static ProductItem Create(ProductId id, ProductNameValue name, PriceValue price, BrandId brandId, CategoryId categoryId)
    {
        var result = new ProductItem(id, name, price, brandId, categoryId);
        result.ApplyChange(new ProductCreated(id, name, price, brandId, categoryId));
        return result;
    }

    public static ProductItem Restore(ProductEntry entry)
    {
        return new ProductItem(new ProductId(entry.Id), new ProductNameValue(entry.Name), new PriceValue(entry.Price), new BrandId(entry.BrandId), new CategoryId(entry.CategoryId), entry.IsDeleted);
    }

    public void Rename(ProductNameValue newName)
    {
        _name = newName;
        ApplyChange(new ProductRenamed(Id, newName));
    }

    public void ChangePrice(PriceValue newPrice)
    {
        _price = newPrice;
        ApplyChange(new ProductPriceChanged(Id, newPrice));
    }

    public void Delete()
    {
        _isDeleted = true;
        ApplyChange(new ProductDeleted(Id));
    }

    public ProductEntry Entry() =>
        new()
        {
            Id = Id.Id,
            Name = _name.Name,
            Price = _price.Price,
            BrandId = _brandId.Id,
            CategoryId = _categoryId.Id,
            IsDeleted = _isDeleted,
        };

    public override string ToString() => $"{_name.Name}";
}