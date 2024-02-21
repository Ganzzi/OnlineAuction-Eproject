using DomainLayer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAdminService
    {
        Task<(int, IList<UserRatingAndBidCount>)> ListAllUserWithRatingAndBidCount(int take,int page);
        Task<bool> LockOrUnlockUser(int userId);
        Task<IList<CategoryItemCount>> ListAllCategoryAndCountItem();
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        
        Task<Item> GetAnItem(int id);
        Task<bool> AddOrDeleteCategoryItem(int CategoryId, int ItemId);
        Task<CategoryWithListItemsResult> CategoryWithListItem(int id, int page, int take, string searchName, bool? belongtocategory);
        Task<(IList<ListItemCategoryBidCount>, int)> ListItemWithCount(int page, int take);
        Task<ItemWithCategoryListResult> ItemWithListCategory(int id);
    }
}
