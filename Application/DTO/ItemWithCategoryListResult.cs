using DomainLayer.Entities.Models;

public class CategoryWithBelongsToItemResult
{
    public Category Category { get; set; }
    public bool BelongsToItem { get; set; }
}

public class ItemWithCategoryListResult
{
    public Item Item { get; set; }
    public CategoryWithBelongsToItemResult[] Categories { get; set; }
}
