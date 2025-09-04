using TSEasy.Catalog.DataAccess;
using TSEasy.Catalog.Presentation;

namespace TSEasyConsoleApp.Helpers;

public class CommonHelper
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