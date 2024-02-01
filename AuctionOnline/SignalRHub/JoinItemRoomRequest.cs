namespace AuctionOnline.SignalRHub
{
    public class JoinItemRoomRequest
    {
        public int UserId {get; set;}
        public string? Username {get; set;}
        public int ItemId {get; set;}
        public string? ItemTitle {get; set;}
        public int? BidAmount {get; set;}
    }
}