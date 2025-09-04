using DMHexagonal.Catalog.Application.CatalogFeature;
using DMHexagonal.Catalog.Infra.Adapters;
using DMHexagonal.Catalog.Infra.Tools;
using DMHexagonal.Catalog.Presentation.Scripts;
using DMHexagonalConsoleApp.Scripts;

namespace DMHexagonalConsoleApp.Helpers;

public static class MasterHelper
{
    private static CategoryStorage _categoryStorage;
    private static BrandStorage _brandStorage;
    private static ProductStorage _productStorage;
    private static readonly DomainEventDispatcher _dispatcher = new ();

    static MasterHelper()
    {
        _brandStorage = new BrandStorage(_dispatcher);
        _categoryStorage = new CategoryStorage(_dispatcher);
        _productStorage = new ProductStorage(_dispatcher);
        DeveloperScript.Run(_dispatcher);
        _dispatcher.Run();
    }

    public static BrandScript CreateBrandScript()
    {
        var service = new BrandApplicationService(_brandStorage);
        return new BrandScript(service);
    }

    public static CategoryScript CreateCategoryScript()
    {
        var service = new CategoryApplicationService(_categoryStorage);
        return new CategoryScript(service);
    }

    public static ProductScript CreateProductScript()
    {
        var service = new ProductApplicationService(_productStorage);
        return new ProductScript(service);
    }

    public static CommonScript CreateCommonScript()
    {
        var service = new CommonApplicationService(_productStorage, _brandStorage, _categoryStorage);
        return new CommonScript(service);
    }
}