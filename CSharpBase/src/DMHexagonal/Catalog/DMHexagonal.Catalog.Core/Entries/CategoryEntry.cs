namespace DMHexagonal.Catalog.Core.Entries;

public class CategoryEntry
{
    public Guid Id { get; set; }

    public string Name { get; init; } = string.Empty;

    public bool IsDeleted { get; init; }
}