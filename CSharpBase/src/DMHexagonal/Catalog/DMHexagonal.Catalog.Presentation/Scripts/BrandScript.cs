using DMHexagonal.Catalog.Application.CatalogFeature;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Presentation.Scripts;

public class BrandScript(BrandApplicationService service)
{
    public Result<IEnumerable<BrandItem>> AllItems()
    {
        return service.AllItems();
    }

    public Result<int> AddItem(string name)
    {
        var result = service.AddItem(new BrandNameValue(name))
            .Throw(fail => throw new ApplicationException(fail.Error));

        return Result.Ok(result.Value);
    }

    public Result ChangeName(int id, string newName)
    {
        return service.ChangeName(new BrandId(id), new BrandNameValue(newName));
    }

    public Result DeleteItem(int id)
    {
        return service.DeleteItem(new BrandId(id));
    }
}