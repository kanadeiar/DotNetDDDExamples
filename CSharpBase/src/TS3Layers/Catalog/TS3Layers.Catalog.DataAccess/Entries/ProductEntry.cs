namespace TS3Layers.Catalog.DataAccess.Entries;

public class ProductEntry
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int BrandId { get; init; }

    public int CategoryId { get; init; }

    public decimal Price { get; init; }
}