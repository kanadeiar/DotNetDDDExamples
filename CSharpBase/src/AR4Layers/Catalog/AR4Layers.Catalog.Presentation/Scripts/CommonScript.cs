using AR4Layers.Catalog.Services.Services;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Presentation.Scripts;

public class CommonScript
{
    private readonly ProductApplicationService _productService = new();
    private readonly BrandApplicationService _brandService = new();
    private readonly CategoryApplicationService _categoryService = new();

    public Result InitDemo()
    {
        var categories = new CategoryScript();
        var c1 = categories.AddItem("Шапки").TryGetValue(f => throw new ApplicationException());
        var c2 = categories.AddItem("Куртки").TryGetValue(f => throw new ApplicationException());
        var c3 = categories.AddItem("Штаны").TryGetValue(f => throw new ApplicationException());
        var brands = new BrandScript();
        var b1 = brands.AddItem("Adidas").TryGetValue(f => throw new ApplicationException());
        var b2 = brands.AddItem("Nike").TryGetValue(f => throw new ApplicationException());
        var b3 = brands.AddItem("Reebok").TryGetValue(f => throw new ApplicationException());
        var products = new ProductScript();
        products.AddItem("Шапка-ушанка", 100, b1, c1);
        products.AddItem("Спортивная карта", 5000, b2, c2);
        products.AddItem("Карсные штаны", 100, b3, c3);

        return Result.Ok();
    }

    public Result<IEnumerable<string>> AllProducts()
    {
        var brands = _brandService.AllItems()
            .Throw(f => throw new ApplicationException())
            .ToDictionary(p => p.Id, p => p);
        var categories = _categoryService.AllItems()
            .Throw(f => throw new ApplicationException())
            .ToDictionary(p => p.Id, p => p);

        var items = _productService.AllItems()
            .Throw(f => throw new ApplicationException())
            .Select(p => new { p, e = p.Entry() })
            .Select(tuple => $"{categories[tuple.e.CategoryId]} {brands[tuple.e.CategoryId]} - {tuple.p.FullText()}");

        return Result.Ok(items);
    }
}