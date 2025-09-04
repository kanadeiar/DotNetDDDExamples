namespace DMHexagonal.Catalog.Core.Entries;

public class ProductEntry
{
    public Guid Id { get; set; }

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public Guid BrandId { get; init; }

    public Guid CategoryId { get; init; }

    public bool IsDeleted { get; init; }
}