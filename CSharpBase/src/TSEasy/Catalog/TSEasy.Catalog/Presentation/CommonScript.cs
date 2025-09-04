using Kanadeiar.Common.Functionals;
using TSEasy.Catalog.Core;
using TSEasy.Catalog.DataAccess;

namespace TSEasy.Catalog.Presentation;

public class CommonScript(ProductStorage productStorage, BrandStorage brandStorage, CategoryStorage categoryStorage)
{
    public Result InitDemo()
    {
        var categories = new CategoryScript(categoryStorage);
        var c1 = categories.AddItem("Шапки").TryGetValue(f => throw new ApplicationException());
        var c2 = categories.AddItem("Куртки").TryGetValue(f => throw new ApplicationException());
        var c3 = categories.AddItem("Штаны").TryGetValue(f => throw new ApplicationException());
        var brands = new BrandScript(brandStorage);
        var b1 = brands.AddItem("Adidas").TryGetValue(f => throw new ApplicationException());
        var b2 = brands.AddItem("Nike").TryGetValue(f => throw new ApplicationException());
        var b3 = brands.AddItem("Reebok").TryGetValue(f => throw new ApplicationException());
        var products = new ProductScript(productStorage);
        products.AddItem("Шапка-ушанка", 100, b1, c1);
        products.AddItem("Спортивная карта", 5000, b2, c2);
        products.AddItem("Карсные штаны", 100, b3, c3);

        return Result.Ok();
    }

    public Result<IEnumerable<string>> AllProducts()
    {
        var brands = brandStorage.Load(e => !e.IsDeleted).ToDictionary(b => b.Id, BrandItem.Restore);
        var categories = categoryStorage.Load(e => !e.IsDeleted).ToDictionary(b => b.Id, CategoryItem.Restore);

        var items = productStorage.Load(e => !e.IsDeleted)
            .Select(e => new { e, p = ProductItem.Restore(e) })
            .Select(tuple => $"{categories[tuple.e.CategoryId]} {brands[tuple.e.BrandId]} - {tuple.p.FullText()}");

        return Result.Ok(items);
    }
}