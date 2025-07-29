using AR4Layers.Catalog.DataAccess.Data;

namespace AR4Layers.Catalog.DataAccess;

public class Registry
{
    private static Registry _inst = new();

    internal ProductStorage productStorage = new();
    internal BrandStorage brandStorage = new();
    internal CategoryStorage categoryStorage = new();

    public static void InitFake(FakeRegistry fake) => _inst = fake;

    public static ProductStorage ProductStorage => _inst.productStorage;
    public static BrandStorage BrandStorage => _inst.brandStorage;
    public static CategoryStorage CategoryStorage => _inst.categoryStorage;
}

public class FakeRegistry : Registry
{
    public ProductStorage FakeProductStorage
    {
        get => productStorage;
        set => productStorage = value;
    }

    public BrandStorage FakeBrandStorage
    {
        get => brandStorage;
        set => brandStorage = value;
    }

    public CategoryStorage FakeCategoryStorage
    {
        get => categoryStorage;
        set => categoryStorage = value;
    }
}