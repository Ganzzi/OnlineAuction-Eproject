using System.Collections.Concurrent;

namespace AuctionOnline.SignalRHub
{
    public class SharedDb
    {
        private readonly ConcurrentDictionary<int, IList<int>> roomsOfUser = new ConcurrentDictionary<int, IList<int>>();
        public ConcurrentDictionary<int, IList<int>> UserToRooms => roomsOfUser;
    }
}