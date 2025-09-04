using DMHexagonal.Catalog.Application.CatalogFeature;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.ProductAggregate;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Presentation.Scripts;

public class ProductScript(ProductApplicationService service)
{
    public Result<IEnumerable<ProductItem>> AllItems()
    {
        return service.AllItems();
    }

    public Result<IEnumerable<ProductItem>> Filter(string name = "", int brandId = 0, int categoryId = 0)
    {
        var items = service.AllItems()
            .Throw(f => throw new ApplicationException())
            .Where(p => p.Filter(name, brandId, categoryId));

        return Result.Ok(items);
    }

    public Result<int> AddItem(string name, decimal price, int brandId, int categoryId)
    {
        var result = service.AddItem(new ProductNameValue(name), new PriceValue(price), new BrandId(brandId), new CategoryId(categoryId))
            .Throw(fail => throw new ApplicationException(fail.Error));

        return Result.Ok(result.Value);
    }

    public Result ChangeName(int id, string newName)
    {
        return service.ChangeName(new ProductId(id), new ProductNameValue(newName));
    }

    public Result ChangePrice(int id, decimal newPrice)
    {
        return service.ChangePrice(new ProductId(id), new PriceValue(newPrice));
    }

    public Result DeleteItem(int id)
    {
        return service.DeleteItem(new ProductId(id));
    }
}