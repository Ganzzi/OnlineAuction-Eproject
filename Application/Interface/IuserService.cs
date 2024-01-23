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
        Task<(IList<CategoryItem>, int)> searchItem(int page, int take, string? search, int? cate);
        Task<int> sellItem(SellItemReqest req);
        Task<bool> Ratting(string username, RateBuyerRequest req);
        Task<bool> updateItem(SellItemReqest req);
        Task<AuctionHistory> GetAcutionHistory(string username, int id);
        Task<bool> PlaceABid(PlaceBidRequest req, string username);
        Task<(User, int)> getProfileDetail(string username);
    }
}
