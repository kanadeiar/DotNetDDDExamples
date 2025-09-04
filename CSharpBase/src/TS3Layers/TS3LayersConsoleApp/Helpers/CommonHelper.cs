using TS3Layers.Catalog.DataAccess.Data;
using TS3Layers.Catalog.Presentation.Scripts;

namespace TS3LayersConsoleApp.Helpers;

public static class CommonHelper
{
    private static readonly CategoryStorage _categoryStorage = new();
    private static readonly BrandStorage _brandStorage = new();
    private static readonly ProductStorage _productStorage = new();

    public static CommonScript CreateGeneralScript()
    {
        return new CommonScript(_productStorage, _brandStorage, _categoryStorage);
    }

    public static ProductScript CreateScript()
    {
        return new ProductScript(_productStorage);
    }

    public static CategoryScript CreateCategoryScript()
    {
        return new CategoryScript(_categoryStorage);
    }

    public static BrandScript CreateBrandScript()
    {
        return new BrandScript(_brandStorage);
    }
}