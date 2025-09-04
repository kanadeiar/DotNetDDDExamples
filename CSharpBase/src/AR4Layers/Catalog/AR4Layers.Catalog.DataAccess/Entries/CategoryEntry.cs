namespace AR4Layers.Catalog.DataAccess.Entries;

public class CategoryEntry
{
    public int Id { get; set; }

    public string Name { get; init; } = string.Empty;

    public bool IsDeleted { get; init; }
}