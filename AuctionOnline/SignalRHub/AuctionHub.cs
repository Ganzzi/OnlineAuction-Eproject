using Microsoft.AspNetCore.SignalR;

namespace AuctionOnline.SignalRHub
{
    public class AuctionHub : Hub
    {
        private readonly SharedDb _sharedDb;
        public AuctionHub(SharedDb sharedDb)
        {
            _sharedDb = sharedDb;
        }

        public async Task OnUserConnected(int UserId)
        {
            if (_sharedDb.UserToRooms.TryGetValue(UserId, out var itemRooms) && itemRooms != null)
            {
                foreach (var item in itemRooms)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"item_{item}");
                }
            }
        }


        public async Task OnUserDisconnected(int UserId)
        {
            if (_sharedDb.UserToRooms.TryGetValue(UserId, out var itemRooms) && itemRooms != null)
            {
                foreach (var item in itemRooms)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"item_{item}");
                }
            }
        }

        public async Task JoinItemRoom(JoinItemRoomRequest req)
        {
            if (!_sharedDb.UserToRooms.TryGetValue(req.UserId, out var itemRooms))
            {
                itemRooms = new List<int>();
                _sharedDb.UserToRooms.TryAdd(req.UserId, itemRooms);
            }

            if (!itemRooms.Contains(req.ItemId))
            {
                itemRooms.Add(req.ItemId);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"item_{req.ItemId}");

            var x = Clients.Group($"item_{req.ItemId}");
            await x
                .SendAsync("someonejoinitemroom", req.ItemId, req.UserId, req.BidAmount ?? 0);
        }


    }
}