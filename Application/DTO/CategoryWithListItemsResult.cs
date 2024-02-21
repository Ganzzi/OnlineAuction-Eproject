using DomainLayer.Entities.Models;

public class CategoryWithListItemsResult
{
    public Category Category { get; set; }
    public IList<ItemWithBelongsToCategoryResult> Items { get; set; }
    public int Count { get; set; }
}

public class ItemWithBelongsToCategoryResult
{
    public Item Item { get; set; }
    public bool BelongsToCategory { get; set; }
}