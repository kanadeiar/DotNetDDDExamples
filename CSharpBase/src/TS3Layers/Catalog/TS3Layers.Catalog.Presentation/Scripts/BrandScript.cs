using Kanadeiar.Common.Functionals;
using TS3Layers.Catalog.Core.BrandModule;
using TS3Layers.Catalog.DataAccess.Data;

namespace TS3Layers.Catalog.Presentation.Scripts;

public class BrandScript(BrandStorage storage)
{
    public Result<IEnumerable<BrandItem>> AllItems()
    {
        var items = storage.All().Select(BrandItem.Restore);

        return Result.Ok(items);
    }

    public Result<int> AddItem(string name)
    {
        try
        {
            storage.BeginTransaction();
            var id = storage.NextIdentity();

            var item = new BrandItem(id, name);

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