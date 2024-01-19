using Application.Interface;
using DomainLayer.Core;
using DomainLayer.Entities.Models;
using DomainLayer.SpecificationPattern;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    internal class UserService:IuserService
    {
        private readonly IUnitOfWork _u;
        public UserService(IUnitOfWork u)
        {
            _u = u;
        }

        // categorylist 
        public async Task<IList<Category>> categorylist()
        {
            try
            {
                var categoryspec = new BaseSpecification<Category>();
                var liscategory = await _u.Repository<Category>().ListAllAsync();

                foreach (var item in liscategory)
                {
                    var categoryItemSpec = new BaseSpecification<CategoryItem>(x => x.CategoryId == item.CategoryId)
                        .AddInclude(x => x.Include(x => x.Item).ThenInclude(x => x.Bids));
                    var categoryItemList = await _u.Repository<CategoryItem>().FindOne(categoryItemSpec);
                    var TopTen = categoryItemList.Item.Bids.OrderByDescending(x => x.BidAmout).Take(10);
                }
                return liscategory;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        //UserProfile ****
        public async Task<User> getUser(string username)
        {
            var spec = new BaseSpecification<User>(x => x.Name == username);
            var User = await _u.Repository<User>().FindOne(spec);
            try
            {
                if (User == null)
                {
                    return null;
                }
                else
                {
                    return User;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // update User
        public async Task<User> UpdateUser(User model)
        {
            try
            {
                var Userspec = new BaseSpecification<User>(x => x.Name == model.Name);
                var User = await _u.Repository<User>().FindOne(Userspec);
                if (User != null)
                {
                    User.Name = model.Name;
                    User.Email = model.Email;
                    User.Password = model.Password;
                    await _u.SaveChangesAsync();
                    return User;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                await _u.RollBackChangesAsync();
                return null;
            }

        }

        //item list with search query
        public async Task<(IList<CategoryItem>, int)> searchItem(int page, int take, string? search, int? cate)
        {
            try
            {
                var skip = take * (page - 1);

                BaseSpecification<CategoryItem> itemSpec;

                if (!string.IsNullOrEmpty(search))
                {
                    itemSpec  = new BaseSpecification<CategoryItem>(x => x.Item.Title.Contains(search));
                } else if (cate.HasValue)
                {
                    itemSpec  = new BaseSpecification<CategoryItem>(x => x.CategoryId == cate.Value);
                } else {
                    itemSpec  = new BaseSpecification<CategoryItem>();
                }
                
                var count = await _u.Repository<CategoryItem>().CountAsync(itemSpec);

                itemSpec = itemSpec.ApplyPaging(skip, take)
                        .AddInclude(x => x.Include(x => x.Item).ThenInclude(x => x.Bids));
                        // .ApplyOrderBy(x => x.StartDate);
                
                var listItem = await _u.Repository<CategoryItem>().ListAsynccheck(itemSpec);

                return (listItem, count);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return (null, 0);
            }
        }

        // tbc
        //public async Task<int> tbc(int? id)
        //{
        //    float avg = 0;
        //    float sum = 0;
        //    var specitem = new BaseSpecification<Rating>(x => x.ItemId == id);
        //    var listitem = await _u.Repository<Rating>().ListAsynccheck(specitem);
        //    foreach (Rating rating in listitem)
        //    {
        //        sum = +rating.;
        //    }
        //    avg = sum / listitem.Count;
        //    return (int)avg;
        //}


        //  get item by id
        public async Task<Item> getItemById(int id)
        {
            try
            {
                var itemspec = new BaseSpecification<Item>(x => x.ItemId == id)
                    .AddInclude(x => x.Include(x => x.Bids).Include(x => x.AuctionHistory).Include(x => x.CategoryItems));
                var item = await _u.Repository<Item>().FindOne(itemspec);
                if (item != null)
                {
                    return item;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        // sell item
        public async Task<int> sellItem(SellItemReqest req)
        {

            try
            {
                var checkName = new BaseSpecification<Item>(x => x.Title == req.Item.Title);
                if (checkName != null)
                {
                    return 0;
                }
                else
                {
                    var additem = await _u.Repository<Item>().AddAsync(req.Item);
                    await _u.SaveChangesAsync();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                await _u.RollBackChangesAsync();
                return -1;
            }
        }

        // Rating
        public async Task<bool> Ratting(string username, RateBuyerRequest req)
        {
            try
            {
                var Userspec = new BaseSpecification<User>(x => x.Name == username);
                var user = await _u.Repository<User>().FindOne(Userspec);
                var itemspec = new BaseSpecification<Item>(x => x.ItemId == req.ItemId);
                var item = await _u.Repository<Item>().FindOne(itemspec);
                if (user == null && item == null)
                {
                    return false;
                }
                else
                {
                    var Ratting = new Rating();
                    Ratting.RatingDate = DateTime.Now;
                    Ratting.RaterId = user.UserId;
                    Ratting.RatedUserId = req.RatedUserId;
                    Ratting.ItemId = req.ItemId;
                    Ratting.Rate = req.RatingAmount;
                    await _u.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                await _u.RollBackChangesAsync();
                return false;
            }

        }
    }
}
