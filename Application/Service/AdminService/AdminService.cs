using Application.Interface;
using DomainLayer.Core;
using DomainLayer.Entities.Models;
using DomainLayer.SpecificationPattern;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.AdminServicevice
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _u;
        private readonly RedisService _redis;

        public AdminService(
            IUnitOfWork u,
            RedisService redis
        )
        {
            _u = u;
            _redis = redis;
        }

        // gửi về 10 user + avgRate
        public async Task<(int, IList<UserRatingAndBidCount>)> ListAllUserWithRatingAndBidCount(int take, int page)
        {
            var skip = take * (page - 1);
            var key1 = $"admin_{page}_{skip}";
            var key2 = $"{key1}_count";

            var cacheData1 = await _redis.GetCachedData<UserRatingAndBidCount[]>(key1);
            var cacheData2 = await _redis.GetCachedData<int>(key2);

            if (cacheData1 != null && cacheData2 != null)
            {
                return (cacheData2, cacheData1);
            }

            try
            {
                var userspec = new BaseSpecification<User>();
                var countUser = await _u.Repository<User>().CountAsync(userspec);

                userspec = userspec.ApplyPaging(skip, take);
                var listUser = await _u.Repository<User>().ListAsynccheck(userspec);

                var userRatingAndBidCounts = new List<UserRatingAndBidCount>();

                foreach (var user in listUser)
                {
                    var ratingSpec = new BaseSpecification<Rating>(x => x.RaterId == user.UserId);
                    var userRatings = await _u.Repository<Rating>().CountAsync(ratingSpec);

                    var ratedspec = new BaseSpecification<Rating>(x => x.RatedUserId == user.UserId);
                    var Rateds = await _u.Repository<Rating>().ListAsynccheck(ratedspec);

                    var avgRate = Rateds != null && Rateds.Count > 0 ? (int)Rateds.ToArray().Average(x => x.Rate) : -1;

                    var bidSpec = new BaseSpecification<Bid>(x => x.UserId == user.UserId);
                    var userBids = await _u.Repository<Bid>().CountAsync(bidSpec);

                    userRatingAndBidCounts.Add(new UserRatingAndBidCount
                    {
                        User = user,
                        Ratings = userRatings,
                        AvgRate = avgRate,
                        BidCount = userBids
                    });
                }

                await _redis.SetCachedData<UserRatingAndBidCount[]>(key1, userRatingAndBidCounts.ToArray(), TimeSpan.FromSeconds(180));
                await _redis.SetCachedData<int>(key2, countUser, TimeSpan.FromSeconds(180));

                return (countUser, userRatingAndBidCounts);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        // cate và itemcount
        public async Task<IList<CategoryItemCount>> ListAllCategoryAndCountItem()
        {
            var key = "admin_category_item_count";
            var cachedData = await _redis.GetCachedData<List<CategoryItemCount>>(key);
            if (cachedData != null)
            {
                return cachedData;
            }

            try
            {
                var listCate = await _u.Repository<Category>().ListAllAsync();
                var categoryItemCountList = new List<CategoryItemCount>();

                foreach (var cate in listCate)
                {
                    var spec = new BaseSpecification<CategoryItem>(x => x.CategoryId == cate.CategoryId);
                    var itemCount = await _u.Repository<CategoryItem>().CountAsync(spec);
                    categoryItemCountList.Add(new CategoryItemCount
                    {
                        Category = cate,
                        ItemCount = itemCount
                    });
                }

                await _redis.SetCachedData<CategoryItemCount[]>(key, categoryItemCountList.ToArray(), TimeSpan.FromSeconds(180));
                return categoryItemCountList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        // lock user
        public async Task<bool> LockOrUnlockUser(int userId)
        {
            try
            {
                var spec = new BaseSpecification<User>(x => x.UserId == userId);
                var user = await _u.Repository<User>().FindOne(spec);

                if (user.Role == "User")
                {
                    user.Role = "Disable";
                    await _u.SaveChangesAsync();
                    return true;
                }
                else if (user.Role == "Disable")
                {
                    user.Role = "User";
                    await _u.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                await _u.RollBackChangesAsync();
                return false;
            }
        }

        // Create Category
        public async Task<bool> CreateCategory(Category category)
        {
            try
            {
                var addcate = await _u.Repository<Category>().AddAsync(new Category
                {
                    CategoryName = category.CategoryName,
                    Description = category.Description
                });
                await _u.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _u.RollBackChangesAsync();
                return false;
            }
        }

        //update category
        public async Task<bool> UpdateCategory(Category category)
        {
            try
            {
                var spec = new BaseSpecification<Category>(x => x.CategoryId == category.CategoryId);
                var oldcategory = await _u.Repository<Category>().FindOne(spec);
                oldcategory.CategoryName = category.CategoryName;
                oldcategory.Description = category.Description;
                _u.Repository<Category>().Update(oldcategory);
                await _u.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // get item by Id 
        public async Task<Item> GetAnItem(int id)
        {
            var key = $"item_{id}";
            var cachedItem = await _redis.GetCachedData<Item>(key);

            if (cachedItem != null)
            {
                return cachedItem;
            }

            try
            {
                var spec = new BaseSpecification<Item>(x => x.ItemId == id);
                var item = await _u.Repository<Item>().FindOne(spec);

                if (item != null)
                {
                    await _redis.SetCachedData<Item>(key, item, TimeSpan.FromSeconds(180));
                }

                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // get category details have itemlist
        public async Task<CategoryWithListItemsResult> CategoryWithListItem(int id, int page, int take, string searchName, bool? belongToCategory)
        {
            var key = $"category_{id}_page_{page}_take_{take}_search_{searchName}_belongToCategory_{belongToCategory}";
            var cachedResult = await _redis.GetCachedData<CategoryWithListItemsResult>(key);

            if (cachedResult != null)
            {
                return cachedResult;
            }

            try
            {
                var skip = take * (page - 1);
                var categorySpec = new BaseSpecification<Category>(x => x.CategoryId == id);
                var category = await _u.Repository<Category>().FindOne(categorySpec);

                BaseSpecification<Item> itemSpec = new BaseSpecification<Item>(
                        ci => ci.Title.Contains(searchName)
                    );


                if (belongToCategory != null)
                {
                    if (belongToCategory == true)
                    {
                        itemSpec = itemSpec = new BaseSpecification<Item>(
                            ci => ci.Title.Contains(searchName) && ci.CategoryItems != null && ci.CategoryItems.Any(ci => ci.CategoryId == id)
                        );
                    }
                    else
                    {
                        itemSpec = itemSpec = new BaseSpecification<Item>(
                            ci => ci.Title.Contains(searchName) && ci.CategoryItems != null && !ci.CategoryItems.Any(ci => ci.CategoryId == id)
                        );
                    }
                }

                itemSpec = itemSpec.AddInclude(query => query?.Include(x => x.CategoryItems));

                var count = await _u.Repository<Item>().CountAsync(itemSpec);

                itemSpec = itemSpec.ApplyPaging(skip, take);

                var listItems = await _u.Repository<Item>().ListAsynccheck(itemSpec);
                var itemResults = listItems.Select(item =>
                {
                    var belongs = item.CategoryItems != null && item.CategoryItems.Any(x => x.CategoryId == id);
                    return new ItemWithBelongsToCategoryResult { Item = item, BelongsToCategory = belongs };
                }).ToList();

                var result = new CategoryWithListItemsResult { Category = category, Items = itemResults, Count = count };

                await _redis.SetCachedData<CategoryWithListItemsResult>(key, result, TimeSpan.FromSeconds(180));

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // add_or_remove_item
        public async Task<bool> AddOrDeleteCategoryItem(int CategoryId, int ItemId)
        {
            var spec = new BaseSpecification<CategoryItem>(x => x.CategoryId == CategoryId && x.ItemId == ItemId);

            try
            {
                var categoryItem = await _u.Repository<CategoryItem>().FindOne(spec);
                if (categoryItem == null)
                {
                    await _u.Repository<CategoryItem>().AddAsync(new CategoryItem
                    {
                        CategoryId = CategoryId,
                        ItemId = ItemId
                    });
                }
                else
                {
                    _u.Repository<CategoryItem>().Delete(categoryItem);
                }

                await _u.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _u.RollBackChangesAsync();
                return false;
            }
        }

        //list item + count category, count bid, count page
        public async Task<(IList<ListItemCategoryBidCount>, int)> ListItemWithCount(int page, int take)
        {
            var key = $"item_with_count_{page}_{take}";
            var cachedData = await _redis.GetCachedData<ListItemCategoryBidCount[]>(key);
            var cachedCount = await _redis.GetCachedData<int>($"{key}_count");

            if (cachedData != null && cachedCount != null)
            {
                return (cachedData, cachedCount);
            }

            try
            {
                var skip = take * (page - 1);
                var spec = new BaseSpecification<Item>();
                var count = await _u.Repository<Item>().CountAsync(spec);

                spec = spec
                    .AddInclude(q => q.Include(i => i.Seller))
                    .ApplyPaging(skip, take);
                var listItems = await _u.Repository<Item>().ListAsynccheck(spec);

                var itemRatingAndBidCounts = new List<ListItemCategoryBidCount>();
                foreach (var item in listItems)
                {
                    var categorySpec = new BaseSpecification<CategoryItem>(x => x.ItemId == item.ItemId);
                    var categoryCount = await _u.Repository<CategoryItem>().CountAsync(categorySpec);

                    var bidSpec = new BaseSpecification<Bid>(x => x.ItemId == item.ItemId);
                    var bidCount = await _u.Repository<Bid>().CountAsync(bidSpec);

                    itemRatingAndBidCounts.Add(new ListItemCategoryBidCount
                    {
                        Item = item,
                        CategoryCount = categoryCount,
                        BidCount = bidCount
                    });
                }

                await _redis.SetCachedData<ListItemCategoryBidCount[]>(key, itemRatingAndBidCounts.ToArray(), TimeSpan.FromSeconds(180));
                await _redis.SetCachedData<int>($"{key}_count", count, TimeSpan.FromSeconds(180));

                return (itemRatingAndBidCounts, count);
            }
            catch (Exception ex)
            {
                return (null, 0);
            }
        }

        //thong tin chi tiet item + listcategoryItem(page,take)
        public async Task<ItemWithCategoryListResult> ItemWithListCategory(int id)
        {
            try
            {
                // Check if data exists in Redis cache
                var cachedData = await _redis.GetCachedData<ItemWithCategoryListResult>($"item_with_category_{id}");
                if (cachedData != null)
                {
                    return cachedData;
                }

                // Get item by id
                var itemSpec = new BaseSpecification<Item>(x => x.ItemId == id);
                var item = await _u.Repository<Item>().FindOne(itemSpec);

                var categorySpec = new BaseSpecification<Category>().AddInclude(
                    q => q.Include(ci => ci.CategoryItems)
                );
                var categories = await _u.Repository<Category>().ListAsynccheck(categorySpec);
                var categoryResults = new List<CategoryWithBelongsToItemResult>();

                foreach (var category in categories)
                {
                    var belongsToItem = category.CategoryItems != null ?
                        category.CategoryItems.Any(x => x.ItemId == id) :
                        false;

                    categoryResults.Add(new CategoryWithBelongsToItemResult
                    {
                        Category = category,
                        BelongsToItem = belongsToItem
                    });
                }

                // Create result object
                var result = new ItemWithCategoryListResult
                {
                    Item = item,
                    Categories = categoryResults.ToArray()
                };

                // Store data in Redis cache
                await _redis.SetCachedData($"item_with_category_{id}", result, TimeSpan.FromSeconds(180));

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
