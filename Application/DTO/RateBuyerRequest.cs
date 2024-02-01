using System.ComponentModel.DataAnnotations;

public class RateBuyerRequest
{
    public int ItemId {get; set;}
    public int RatedUserId {get; set;}

    [Range(0, 5, ErrorMessage = "rate must be between {1} and {2}.")]
    public int RatingAmount {get; set;}
    
}