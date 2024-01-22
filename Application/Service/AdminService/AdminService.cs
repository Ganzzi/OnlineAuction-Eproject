using Application.Interface;
using Azure;
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
        // gửi về 10 user
        public async Task<IDictionary<string, (int, int)>> ListAllUserWithRatingAndBidCount(int take, int page)
        {
            try
            {

                var skip = take * (page - 1);
                var userspec = new BaseSpecification<User>().ApplyPaging(skip, take);
                var listUser = await _u.Repository<User>().ListAsynccheck(userspec);
                var userRatingAndBidCounts = new Dictionary<string, (int, int)>();

                foreach (var user in listUser)
                {
                    var ratingSpec = new BaseSpecification<Rating>(x => x.RaterId == user.UserId);
                    var userRatings = await _u.Repository<Rating>().ListAsynccheck(ratingSpec);

                    var bidSpec = new BaseSpecification<Bid>(x => x.UserId == user.UserId);
                    var userBids = await _u.Repository<Bid>().ListAsynccheck(bidSpec);
                    userRatingAndBidCounts[user.Name] = (userRatings.Count, userBids.Count);
                }

                return userRatingAndBidCounts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // cate và itemcount
        public async Task<IDictionary<string, int>> ListAllCategoryAndCountItem()
        {
            try
            {
                var listCate = await _u.Repository<Category>().ListAllAsync();
                var userRatingAndBidCounts = new Dictionary<string, int>();

                foreach (var cate in listCate)
                {
                    var spec = new BaseSpecification<CategoryItem>(x => x.CategoryId == cate.CategoryId);
                    var itemCount = await _u.Repository<CategoryItem>().CountAsync(spec);
                    userRatingAndBidCounts.Add(cate.CategoryName, itemCount);
                }

                return userRatingAndBidCounts;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        // lock user
        public async Task<bool> LockOrUnlock(string username, string status)
        {
            try
            {
                var spec = new BaseSpecification<User>(x => x.Name == username);
                var user = await _u.Repository<User>().FindOne(spec);
                if (status == "Disable")
                {
                    user.Role = "Disable";
                    await _u.SaveChangesAsync();
                    return true;
                }
                else if (status == "User")
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
                var addcate = await _u.Repository<Category>().AddAsync(category);
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
        public async Task<(IList<CategoryItem>, int)> categorylistItem(int id, int page, int take, string searchName, bool belongtocategory)
        {
            try
            {
                var skip = take * (page - 1);
                var catespec = new BaseSpecification<Category>(x => x.CategoryId == id);
                var cate = await _u.Repository<Category>().FindOne(catespec);
                BaseSpecification<CategoryItem> sp;

                if (searchName != null && belongtocategory == false)
                {
                    sp = new BaseSpecification<CategoryItem>(
                        ci => ci.Item.Title.Contains(searchName)
                    );
                }
                else if (belongtocategory == true && searchName == null)
                {
                    sp = sp = new BaseSpecification<CategoryItem>(
                        ci => ci.CategoryId == id
                    );
                }
                else if (belongtocategory == true && searchName != null)
                {
                    sp = sp = new BaseSpecification<CategoryItem>(
                   ci => ci.CategoryId == id && ci.Item.Title.Contains(searchName)
               );
                }
                else
                {

                    return (null, 0);
                }

                sp = sp.AddInclude(query => query.Include(x => x.Item)).ApplyPaging(skip, take);
                var listcategory = await _u.Repository<CategoryItem>().ListAsynccheck(sp);
                var count = await _u.Repository<CategoryItem>().CountAsync(sp);


                return (listcategory, count);
            }
            catch (Exception ex)
            {
                return (null, 0);
            }
        }


        // add_or_remove_item
        public async Task<bool> addOrDeleteItemForCate(int cate, int item, bool status)
        {
            var spec = new BaseSpecification<CategoryItem>(x => x.CategoryId == cate && x.ItemId == item);
            var feildexits = await _u.Repository<CategoryItem>().FindOne(spec);
            try
            {
                if (feildexits != null && status == true)
                {
                    return false;
                }
                else if (feildexits == null && status == true)
                {
                    await _u.Repository<CategoryItem>().AddAsync(feildexits);
                    await _u.SaveChangesAsync();
                    return true;
                }
                else if (feildexits != null && status == false)
                {
                    _u.Repository<CategoryItem>().Delete(feildexits);
                    await _u.SaveChangesAsync();
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                await _u.RollBackChangesAsync();
                return false;
            }
        }


        //list item + count category, count bid, count page
        public async Task<(IDictionary<string, (int, int)>, int)> getListItemhaveCount(int page, int take)
        {
            try
            {
                var skip = take * (page - 1);
                var spec = new BaseSpecification<Item>().ApplyPaging(skip, take);
                var listspec = await _u.Repository<Item>().ListAsynccheck(spec);
                var count = await _u.Repository<Item>().CountAsync(spec);
                var ItemRatingAndBidCounts = new Dictionary<string, (int, int)>();
                foreach (var item in listspec)
                {
                    var ratingSpec = new BaseSpecification<Rating>(x => x.ItemId == item.ItemId);
                    var Ratings = await _u.Repository<Rating>().ListAsynccheck(ratingSpec);
                    var CountRating = tbc(Ratings);

                    var bidSpec = new BaseSpecification<Bid>(x => x.ItemId == item.ItemId);
                    var userBids = await _u.Repository<Bid>().ListAsynccheck(bidSpec);
                    ItemRatingAndBidCounts[item.Title] = (CountRating, userBids.Count);
                }
                return (ItemRatingAndBidCounts, count);
            }
            catch (Exception ex)
            {
                return (null, 0);
            }

        }

        //thong tin chi tiet item + listcategoryItem(page,take)
        public async Task<(Item,IList<CategoryItem>,int)> GetOneItemAndListCategoryItem(int id,int page,int take)
        {
            try { 
                // get item by id
                var Itemspec = new BaseSpecification<Item>(x => x.ItemId == id);
                var Item = await _u.Repository<Item>().FindOne(Itemspec);
                // get listcategoryItem
                var skip = take * (page - 1);
                var CategoryItemspec = new BaseSpecification<CategoryItem>().ApplyPaging(skip, take);
                var count = await _u.Repository<CategoryItem>().CountAsync(CategoryItemspec);
                var listCategoryItem = await _u.Repository<CategoryItem>().ListAsynccheck(CategoryItemspec);
                if (Item != null && listCategoryItem != null)
                {
                    return (Item, listCategoryItem,count);
                }
                else
                {
                    return (null, null,0);
                }

            }
            catch (Exception e )
            {
                return (null,null,0);
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
