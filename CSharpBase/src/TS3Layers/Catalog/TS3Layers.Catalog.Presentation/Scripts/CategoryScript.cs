using Kanadeiar.Common.Functionals;
using TS3Layers.Catalog.Core.CategoryModule;
using TS3Layers.Catalog.DataAccess.Data;

namespace TS3Layers.Catalog.Presentation.Scripts;

public class CategoryScript(CategoryStorage storage)
{
    public Result<IEnumerable<CategoryItem>> AllItems()
    {
        var items = storage.All().Select(CategoryItem.Restore);

        return Result.Ok(items);
    }

    public Result<int> AddItem(string name)
    {
        try
        {
            storage.BeginTransaction();
            var id = storage.NextIdentity();

            var item = new CategoryItem(id, name);

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