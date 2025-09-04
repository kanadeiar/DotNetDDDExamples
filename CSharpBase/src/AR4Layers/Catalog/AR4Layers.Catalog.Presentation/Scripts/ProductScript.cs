using AR4Layers.Catalog.Core.ProductModule;
using AR4Layers.Catalog.Services.Services;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Presentation.Scripts;

public class ProductScript
{
    private readonly ProductApplicationService _service = new();

    public Result<IEnumerable<ProductItem>> AllItems()
    {
        return _service.AllItems();
    }

    public Result<IEnumerable<ProductItem>> Filter(string name = "", int brandId = 0, int categoryId = 0)
    {
        var items = _service.AllItems()
            .Throw(f => throw new ApplicationException())
            .Where(p => p.Filter(name, brandId, categoryId));

        return Result.Ok(items);
    }

    public Result<int> AddItem(string name, decimal price, int bradId, int categoryId)
    {
        return _service.AddItem(name, price, bradId, categoryId);
    }

    public Result ChangeName(int id, string newName)
    {
        return _service.ChangeName(id, newName);
    }

    public Result ChangePrice(int id, decimal newPrice)
    {
        return _service.ChangePrice(id, newPrice);
    }

    public Result DeleteItem(int id)
    {
        return _service.DeleteItem(id);
    }
}