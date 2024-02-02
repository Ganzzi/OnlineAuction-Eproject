using Application.Interface;
using DomainLayer.Core;
using DomainLayer.Entities.Models;
using DomainLayer.SpecificationPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.AdminServicevice
{
    public class AdminService : IAdminServicevice
    {
        private readonly IUnitOfWork _u;

        public AdminService(IUnitOfWork u)
        {
            _u = u;
        }

        // TODO: refactor
        /*
        example: 
            public async Task<List<(User, int, int, int)>> ListAllUser(int take, int page)
            {
                try
                {
                    var skip = take * (page - 1);
                    var userspec = new BaseSpecification<User>().ApplyPaging(skip, take)
                                    .AddInclude(qr => qr.Include(u => u.Ratings).Include(u => u.Bids).Include(u => u.BeingRateds));
                    var listUser = await _u.Repository<User>().ListAsynccheck(userspec);
                    
                    List<(User, int, int, int)> response = null;
                    foreach (var user in listUser)
                    {
                        response.Add(
                            (new User {
                                Name = user.Name,
                                Email = user.Email,
                            }, 
                            user.Ratings.Count,
                            user.BeingRateds.Count,
                            user.Bids.Count
                            )
                        );
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        */

        //new listuser
        public async Task<List<(User, int, int, int)>> ListAllUser(int take, int page)
        {
            try
            {
                var skip = take * (page - 1);
                var userspec = new BaseSpecification<User>().ApplyPaging(skip, take)
                                .AddInclude(qr => qr.Include(u => u.Ratings).Include(u => u.Bids).Include(u => u.BeingRateds));
                var listUser = await _u.Repository<User>().ListAsynccheck(userspec);

                List<(User, int, int, int)> response = null;
                foreach (var user in listUser)
                {
                    response.Add(
                        (new User
                        {
                            Name = user.Name,
                            Email = user.Email,
                        },
                        user.Ratings.Count,
                        user.BeingRateds.Count,
                        user.Bids.Count
                        )
                    );
                }

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // gửi về 10 user + avgRate
        public async Task<(int, IDictionary<User, (int,int, int)>)> ListAllUserWithRatingAndBidCount(int take, int page)
        {
            try
            {

                var skip = take * (page - 1);
                var userspec = new BaseSpecification<User>();
                
                var CountUser =  await _u.Repository<User>().CountAsync(userspec);
                
                userspec = userspec.ApplyPaging(skip, take);
                var listUser = await _u.Repository<User>().ListAsynccheck(userspec);
                var userRatingAndBidCounts = new Dictionary<User, (int,int, int)>();

                foreach (var user in listUser)
                {
                    var ratingSpec = new BaseSpecification<Rating>(x => x.RaterId == user.UserId);
                    var userRatings = await _u.Repository<Rating>().CountAsync(ratingSpec);


                    var ratedspec = new BaseSpecification<Rating>(x => x.RatedUserId == user.UserId);
                    var Rateds = await _u.Repository<Rating>().ListAsynccheck(ratedspec);

                    var avgRate = Rateds != null  && Rateds.Count > 0 ?  Rateds.ToArray().Average(x  => x.Rate) : -1;

                    var bidSpec = new BaseSpecification<Bid>(x => x.UserId == user.UserId);
                    var userBids = await _u.Repository<Bid>().CountAsync(bidSpec);

                    userRatingAndBidCounts[user] = (userRatings, (int)avgRate, userBids);
                }

                return (CountUser, userRatingAndBidCounts);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        // cate và itemcount
        public async Task<IDictionary<Category, int>> ListAllCategoryAndCountItem()
        {
            try
            {
                var listCate = await _u.Repository<Category>().ListAllAsync();
                var userRatingAndBidCounts = new Dictionary<Category, int>();

                foreach (var cate in listCate)
                {
                    var spec = new BaseSpecification<CategoryItem>(x => x.CategoryId == cate.CategoryId);
                    var itemCount = await _u.Repository<CategoryItem>().CountAsync(spec);
                    userRatingAndBidCounts.Add(cate, itemCount);
                }

                return userRatingAndBidCounts;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        // lock user
        public async Task<bool> LockOrUnlock(int userId)
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
                var addcate = await _u.Repository<Category>().AddAsync(new Category{
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
        public async Task<Item> takeOneItem(int id)
        {
            try
            {
                var spec = new BaseSpecification<Item>(x => x.ItemId == id);
                var Item = await _u.Repository<Item>().FindOne(spec);
                return Item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        // get category details have itemlist
        public async Task<(Category, IList<(Item, bool)>, int)> CategorylistItem(int id, int page, int take, string searchName, bool? belongtocategory)
        {
            try
            {
                var skip = take * (page - 1);
                var catespec = new BaseSpecification<Category>(x => x.CategoryId == id);
                var cate = await _u.Repository<Category>().FindOne(catespec);

                BaseSpecification<Item> sp = new BaseSpecification<Item>(
                        ci => ci.Title.Contains(searchName)
                    );


                if (belongtocategory != null) {
                    if(belongtocategory == true) {
                        sp = sp = new BaseSpecification<Item>(
                            ci => ci.Title.Contains(searchName) && ci.CategoryItems.Any(ci => ci.CategoryId == id)
                        );
                    } else {
                        sp = sp = new BaseSpecification<Item>(
                            ci => ci.Title.Contains(searchName) && !ci.CategoryItems.Any(ci => ci.CategoryId == id)
                        );
                    }
                }

                sp = sp.AddInclude(query => query.Include(x => x.CategoryItems));

                var count = await _u.Repository<Item>().CountAsync(sp);
                
                sp = sp.ApplyPaging(skip, take);
                
                var listItems = await _u.Repository<Item>().ListAsynccheck(sp);
                var itemRes = new List<(Item, bool)>();

                foreach (var item in listItems)
                {
                    var belong = item.CategoryItems == null ? false : item.CategoryItems.Any(x => x.CategoryId == id);
                    itemRes.Add((item, belong));
                }

                return (cate, itemRes, count);
            }
            catch (Exception ex)
            {
                return default;
            }
        }


        // add_or_remove_item
        public async Task<bool> addOrDeleteItemForCate(int CategoryId, int ItemId)
        {
            var spec = new BaseSpecification<CategoryItem>(x => x.CategoryId == CategoryId && x.ItemId == ItemId);
            try
            {
                var categoryItem = await _u.Repository<CategoryItem>().FindOne(spec);
                if (categoryItem == null)
                {
                    await _u.Repository<CategoryItem>().AddAsync(new CategoryItem {
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
        public async Task<(IDictionary<Item, (int, int)>, int)> getListItemhaveCount(int page, int take)
        {
            try
            {
                var skip = take * (page - 1);
                var spec = new BaseSpecification<Item>();
                var count = await _u.Repository<Item>().CountAsync(spec);

                spec = spec
                    .AddInclude(q=> q.Include(i => i.Seller))
                    .ApplyPaging(skip, take);
                var listspec = await _u.Repository<Item>().ListAsynccheck(spec);
                var ItemRatingAndBidCounts = new Dictionary<Item, (int, int)>();
                foreach (var item in listspec)
                {
                    var CategorySpec = new BaseSpecification<CategoryItem>(x => x.ItemId == item.ItemId);
                    var Categories = await _u.Repository<CategoryItem>().CountAsync(CategorySpec);

                    var bidSpec = new BaseSpecification<Bid>(x => x.ItemId == item.ItemId);
                    var itemBids = await _u.Repository<Bid>().CountAsync(bidSpec);
                    ItemRatingAndBidCounts[item] = (Categories, itemBids);
                }
                return (ItemRatingAndBidCounts, count);
            }
            catch (Exception ex)
            {
                return (null, 0);
            }

        }

        //thong tin chi tiet item + listcategoryItem(page,take)
        public async Task<(Item, IList<(Category, bool)>)> GetOneItemAndListCategoryItem(int id)
        {
            try
            {
                // get item by id
                var Itemspec = new BaseSpecification<Item>(x => x.ItemId == id);
                var Item = await _u.Repository<Item>().FindOne(Itemspec);
                
                var Categoryspec = new BaseSpecification<Category>().AddInclude(
                    q => q.Include(ci => ci.CategoryItems)
                );
                var Categories = await _u.Repository<Category>().ListAsynccheck(Categoryspec);
                var cateRes = new List<(Category, bool)>();

                foreach (var item in Categories)
                {
                    cateRes.Add((item, item.CategoryItems.Any(x  =>  x.ItemId == id)));
                }
                
                return (Item, cateRes);

            }
            catch (Exception e)
            {
                return default;
            }
        }

        // tinh tbc
        public int tbc(IList<Rating> listrating)
        {
            float avg = 0;
            float sum = 0;
            foreach (Rating rating in listrating)
            {
                sum = +rating.Rate;
            }
            avg = sum / listrating.Count;
            return (int)avg;
        }
    }
}
