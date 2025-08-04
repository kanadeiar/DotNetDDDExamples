using AR4Layers.Catalog.Core.CategoryModule;
using AR4Layers.Catalog.Services.Services;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Presentation.Scripts;

public class CategoryScript
{
    private readonly CategoryApplicationService _service = new();

    public Result<IEnumerable<CategoryItem>> AllItems()
    {
        return _service.AllItems();
    }

    public Result<int> AddItem(string name)
    {
        return _service.AddItem(name);
    }

    public Result ChangeName(int id, string newName)
    {
        return _service.ChangeName(id, newName);
    }

    public Result DeleteItem(int id)
    {
        return _service.DeleteItem(id);
    }
}