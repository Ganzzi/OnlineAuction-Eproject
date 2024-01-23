using DomainLayer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAdminServicevice
    {
        //
        Task<List<(User, int, int, int)>> ListAllUser(int take, int page);
        //
        Task<IDictionary<User, (int, int)>> ListAllUserWithRatingAndBidCount(int take,int page);
        Task<IDictionary<Category, int>> ListAllCategoryAndCountItem();
        Task<bool> LockOrUnlock(int userId);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        
        Task<Item> takeOneItem(int id);
        Task<bool> addOrDeleteItemForCate(int CategoryId, int ItemId);
        Task<(Category, IList<CategoryItem>, int)> CategorylistItem(int id, int page, int take, string searchName, bool? belongtocategory);
        Task<(IDictionary<Item, (int, int)>, int)> getListItemhaveCount(int page, int take);
        Task<(Item, IList<CategoryItem>)> GetOneItemAndListCategoryItem(int id);
    }
}
