namespace AuctionOnline.SignalRHub
{
    public class JoinItemRoomRequest
    {
        public int UserId {get; set;}
        public int ItemId {get; set;}
        public int? BidAmount {get; set;}
    }
}