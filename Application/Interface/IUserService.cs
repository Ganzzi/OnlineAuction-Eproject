using DomainLayer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IUserService
    {
        Task<IList<Category>> Categorylist();
        Task<User> GetUser(string username);
        Task<(User, string)> UpdateUser(User model);
        Task<Item> GetItemById(int id);
        Task<(IList<Item>, int)> SearchItem(int page, int take, string search, string order, int? cate);
        Task<(Item,string)> SellItem(SellItemReqest req);
        Task<(bool,string)> Ratting(string username, RateBuyerRequest req);
        Task<(bool,string)> UpdateItem(SellItemReqest req);
        Task<AuctionHistory> GetAuctionHistory(string username, int id);
        Task<(AuctionHistory, string)> PlaceABid(PlaceBidRequest req, User user);
        Task<(User, int)> GetProfileDetail(string username);
        Task<bool> AuctionEnd(int ItemId);

        Task<IList<User>> GetItemPaticipants(int itemId);
        Task NotifyParticipants(int ItemId, string Content);
    }
}
