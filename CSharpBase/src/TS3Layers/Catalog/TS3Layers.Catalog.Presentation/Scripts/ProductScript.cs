using Kanadeiar.Common.Functionals;
using TS3Layers.Catalog.Core.ProductModule;
using TS3Layers.Catalog.DataAccess.Data;

namespace TS3Layers.Catalog.Presentation.Scripts;

public class ProductScript(ProductStorage storage)
{
    public Result<IEnumerable<ProductItem>> AllItems()
    {
        var items = storage.All().Select(ProductItem.Restore);

        return Result.Ok(items);
    }

    public Result<int> AddItem(string name, int brandId, int categoryId, decimal price)
    {
        try
        {
            storage.BeginTransaction();
            var id = storage.NextIdentity();

            var item = new ProductItem(id, name, brandId, categoryId, price);

            storage.Save(item.Entry());
            storage.Commit();
            return Result.Ok(id);
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail<int>("Не удалось добавить новый элемент. Ошибка: " + e);
        }
    }
}