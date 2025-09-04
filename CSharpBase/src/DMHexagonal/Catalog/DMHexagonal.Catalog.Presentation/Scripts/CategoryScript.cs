using DMHexagonal.Catalog.Application.CatalogFeature;
using DMHexagonal.Catalog.Core.CategoryAggregate;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Presentation.Scripts;

public class CategoryScript(CategoryApplicationService service)
{
    public Result<IEnumerable<CategoryItem>> AllItems()
    {
        return service.AllItems();
    }

    public Result<Guid> AddItem(string name)
    {
        var result = service.AddItem(new CategoryNameValue(name))
            .Throw(fail => throw new ApplicationException(fail.Error));

        return Result.Ok(result.Value);
    }

    public Result ChangeName(Guid id, string newName)
    {
        return service.ChangeName(new CategoryId(id), new CategoryNameValue(newName));
    }

    public Result DeleteItem(Guid id)
    {
        return service.DeleteItem(new CategoryId(id));
    }
}