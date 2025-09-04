using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Application.CatalogFeature;

public class CommonApplicationService(IProductStorage productStorage, IBrandStorage brandStorage, ICategoryStorage categoryStorage)
{
    public Result InitDemo()
    {
        var categories = new CategoryApplicationService(categoryStorage);
        var c1 = categories.AddItem(new CategoryNameValue("Шапки")).TryGetValue(() => throw new ApplicationException());
        var c2 = categories.AddItem(new CategoryNameValue("Куртки")).TryGetValue(() => throw new ApplicationException());
        var c3 = categories.AddItem(new CategoryNameValue("Штаны")).TryGetValue(() => throw new ApplicationException());
        var brands = new BrandApplicationService(brandStorage);
        var b1 = brands.AddItem(new BrandNameValue("Adidas")).TryGetValue(() => throw new ApplicationException());
        var b2 = brands.AddItem(new BrandNameValue("Nike")).TryGetValue(() => throw new ApplicationException());
        var b3 = brands.AddItem(new BrandNameValue("Reebok")).TryGetValue(() => throw new ApplicationException());
        var products = new ProductApplicationService(productStorage);
        products.AddItem(new ProductNameValue("Шапка-ушанка"), new PriceValue(100M), b1, c1);
        products.AddItem(new ProductNameValue("Спортивная карта"), new PriceValue(5000M), b2, c2);
        products.AddItem(new ProductNameValue("Красные штаны"), new PriceValue(30M), b3, c3);
        
        return Result.Ok();
    }

    public Result<IEnumerable<string>> AllProducts()
    {
        var brands = brandStorage.Load(_ => true)
            .ToDictionary(e => e.Id.Value, e => e);
        var categories = categoryStorage.Load(_ => true)
            .ToDictionary(e => e.Id.Value, e => e);

        var items = productStorage.Load(e => !e.IsDeleted)
            .Select(p => new { p, e = p.Entry() })
            .Select(tuple => $"{categories[tuple.e.CategoryId]} {brands[tuple.e.BrandId]} {tuple.p.FullText()}");

        return Result.Ok(items);
    }
}