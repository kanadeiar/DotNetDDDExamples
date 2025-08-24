namespace TSEasy.Catalog.DataAccess.Entries;

public class BrandEntry
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public bool IsDeleted { get; init; }
}