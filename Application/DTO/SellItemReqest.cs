using DomainLayer.Entities.Models;

public class SellItemReqest
{
    public Item Item {set; get;}
    public Category[] Categories {set; get;}
}