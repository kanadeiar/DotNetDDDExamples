using AR4Layers.Catalog.DataAccess.Data;

namespace AR4Layers.Catalog.DataAccess.Registries;

public class DataRegistry
{
    private static readonly DataRegistry _inst = new();

    private ProductStorage _productStorage = new();
    private BrandStorage _brandStorage = new();
    private CategoryStorage _categoryStorage = new();

    public static void InitFake(ProductStorage product) => _inst._productStorage = product;
    public static void InitFake(BrandStorage brand) => _inst._brandStorage = brand;
    public static void InitFake(CategoryStorage category) => _inst._categoryStorage = category;

    public static ProductStorage ProductStorage => _inst._productStorage;
    public static BrandStorage BrandStorage => _inst._brandStorage;
    public static CategoryStorage CategoryStorage => _inst._categoryStorage;
}

