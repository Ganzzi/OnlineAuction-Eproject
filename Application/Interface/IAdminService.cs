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
        Task<IDictionary<string, (int, int)>> ListAllUserWithRatingAndBidCount(int take,int page);
        Task<IDictionary<string, int>> ListAllCategoryAndCountItem();
        Task<bool> LockOrUnlock(string username, string status);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        
        Task<Item> takeOneItem(int id);
        Task<bool> addOrDeleteItemForCate(int cate, int item, bool status);
        Task<(IList<CategoryItem>, int)> categorylistItem(int id, int page, int take, string searchName, bool belongtocategory);
        Task<(IDictionary<string, (int, int)>, int)> getListItemhaveCount(int page, int take);
        Task<(Item, IList<CategoryItem>,int)> GetOneItemAndListCategoryItem(int id, int page, int take);
    }
}
