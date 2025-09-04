using DMHexagonal.Catalog.Application.CatalogFeature;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Presentation.Scripts;

public class CommonScript(CommonApplicationService service)
{
    public Result InitDemo()
    {
        var result = service.InitDemo();

        return result;
    }

    public Result<IEnumerable<string>> AllProducts()
    {
        var results = service.AllProducts();

        return results;
    }
}