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
        Task<(IList<Item>, int)> searchItem(int page, int take, string search, int? cate);
        Task<int> sellItem(Item item);
        Task<bool> Ratting(string username, int id, float rate);
    }
}
