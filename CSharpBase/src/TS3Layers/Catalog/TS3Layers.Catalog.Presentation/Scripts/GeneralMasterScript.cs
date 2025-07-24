using Kanadeiar.Common.Functionals;
using TS3Layers.Catalog.Core.BrandModule;
using TS3Layers.Catalog.Core.CategoryModule;
using TS3Layers.Catalog.Core.ProductModule;
using TS3Layers.Catalog.DataAccess.Data;

namespace TS3Layers.Catalog.Presentation.Scripts;

public class GeneralMasterScript(ProductStorage productStorage, BrandStorage brandStorage, CategoryStorage categoryStorage)
{
    public Result InitDemo()
    {
        var categories = new CategoryScript(categoryStorage);
        var c1 = categories.AddItem("Шапки").TryGetValue(f => throw new ApplicationException());
        var c2 = categories.AddItem("Куртки").TryGetValue(f => throw new ApplicationException());
        var c3 = categories.AddItem("Штаны").TryGetValue(f => throw new ApplicationException());
        var brands = new BrandScript(brandStorage);
        var b1 = brands.AddItem("Adidas").TryGetValue(f => throw new ApplicationException());
        var b2 = brands.AddItem("Adidas").TryGetValue(f => throw new ApplicationException());
        var b3 = brands.AddItem("Adidas").TryGetValue(f => throw new ApplicationException());
        var products = new ProductScript(productStorage);
        products.AddItem("Шапка-ушанка", b1, c1, 100);
        products.AddItem("Спортивная карта", b2, c2, 5000);
        products.AddItem("Карсные штаны", b3, c3, 100);

        return Result.Ok();
    }

    public Result<IEnumerable<string>> AllItems()
    {
        var brands = brandStorage.All().ToDictionary(b => b.Id, BrandItem.Restore);
        var categories = categoryStorage.All().ToDictionary(b => b.Id, CategoryItem.Restore);

        var items = productStorage.All()
            .Select(e => new { e, p = ProductItem.Restore(e) })
            .Select(tuple => $"{categories[tuple.e.CategoryId]} {brands[tuple.e.CategoryId]} - {tuple.p.FullText()}");

        return Result.Ok(items);
    }
}