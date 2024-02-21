using DomainLayer.Entities.Models;

public class UserRatingAndBidCount
{
    public User User { get; set; }
    public int Ratings { get; set; }
    public int AvgRate { get; set; }
    public int BidCount { get; set; }
}