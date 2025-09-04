namespace TSEasy.Catalog.DataAccess.Entries;

public class ProductEntry
{
    public int Id { get; set; }

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public int BrandId { get; init; }

    public int CategoryId { get; init; }

    public bool IsDeleted { get; init; }
}