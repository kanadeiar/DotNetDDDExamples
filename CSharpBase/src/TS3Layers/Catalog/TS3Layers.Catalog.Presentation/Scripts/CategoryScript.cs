using Kanadeiar.Common.Functionals;
using TS3Layers.Catalog.Core.CategoryModule;
using TS3Layers.Catalog.DataAccess.Data;

namespace TS3Layers.Catalog.Presentation.Scripts;

public class CategoryScript(CategoryStorage storage)
{
    public Result<IEnumerable<CategoryItem>> AllItems()
    {
        var items = storage.Load(e => !e.IsDeleted).Select(CategoryItem.Restore);

        return Result.Ok(items);
    }

    public Result<int> AddItem(string name)
    {
        try
        {
            storage.BeginTransaction();

            var item = new CategoryItem(0, name);

            var entry = item.Entry();
            storage.Save(entry);
            storage.Commit();
            return Result.Ok(entry.Id);
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail<int>("Не удалось добавить новый элемент. Ошибка: " + e);
        }
    }

    public Result ChangeName(int id, string newName)
    {
        try
        {
            storage.BeginTransaction();
            var entry = storage.Load(id);
            if (entry is null) throw new ApplicationException("Элемент не найден");
            var item = CategoryItem.Restore(entry);

            item.Rename(newName);

            storage.Save(item.Entry());
            storage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail("Не удалось изменить название элемента. Ошибка: " + e);
        }
    }

    public Result DeleteItem(int id)
    {
        try
        {
            storage.BeginTransaction();
            var entry = storage.Load(id);
            if (entry is null) throw new ApplicationException("Элемент не найден для удаления");
            var item = CategoryItem.Restore(entry);

            item.Delete();

            storage.Save(item.Entry());
            storage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail("Не удалось удалить элемент. Ошибка: " + e);
        }
    }
}