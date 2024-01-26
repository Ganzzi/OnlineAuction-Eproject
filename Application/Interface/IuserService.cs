using DomainLayer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IuserService
    {
        Task<IList<Category>> categorylist();
        Task<User> getUser(string username);
        Task<User> UpdateUser(User model);
        Task<Item> getItemById(int id);
        Task<(IList<Item>, int)> searchItem(int page, int take, string search, string order, int? cate);
        Task<(Item,string)> sellItem(SellItemReqest req);
        Task<bool> Ratting(string username, RateBuyerRequest req);
        Task<(bool,string)> updateItem(SellItemReqest req);
        Task<AuctionHistory> GetAcutionHistory(string username, int id);
        Task<AuctionHistory> PlaceABid(PlaceBidRequest req, User user);
        Task<(User, int)> getProfileDetail(string username);
        Task<bool> AuctionEnd(int ItemId);
    }
}
