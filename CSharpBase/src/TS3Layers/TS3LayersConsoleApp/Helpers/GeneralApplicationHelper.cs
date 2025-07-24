using TS3Layers.Catalog.DataAccess.Data;
using TS3Layers.Catalog.Presentation.Scripts;

namespace TS3LayersConsoleApp.Helpers;

public static class GeneralApplicationHelper
{
    private static readonly CategoryStorage _categoryStorage = new CategoryStorage();
    private static readonly BrandStorage _brandStorage = new BrandStorage();
    private static readonly ProductStorage _productStorage = new ProductStorage();

    public static GeneralMasterScript CreateGeneralScript()
    {
        return new GeneralMasterScript(_productStorage, _brandStorage, _categoryStorage);
    }

    public static CategoryScript CreateCategoryScript()
    {
        return new CategoryScript(_categoryStorage);
    }

    public static BrandScript CreateBrandScript()
    {
        return new BrandScript(_brandStorage);
    }

    public static ProductScript CreateScript()
    {
        return new ProductScript(_productStorage);
    }
}