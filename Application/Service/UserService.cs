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
        public async Task<(IList<Item>, int)> searchItem(int page, int take, string search, int? cate)
        {
            try
            {
                var skip = take * (page - 1);
                var specCount = new BaseSpecification<Item>();
                var Count = await _u.Repository<Item>().CountAsync(specCount);
                if (search != null && cate == null)
                {
                    var Itemspec = new BaseSpecification<Item>(x => x.Title.Contains(search)).ApplyPaging(skip, take);
                    var listItem = await _u.Repository<Item>().ListAsynccheck(Itemspec);
                    foreach (var models in listItem)
                    {
                        var specModel = new BaseSpecification<Item>(x => x.ItemId == models.ItemId).AddInclude(x => x.Include(x => x.Bids).Include(x => x.Rating));
                        var model = await _u.Repository<Item>().FindOne(specModel);
                       
                    }
                    return (listItem, Count);
                }
                else if (search != null && cate != null)
                {
                    var Itemspec = new BaseSpecification<Item>(x => x.Title.Contains(search) && x.ItemId == cate);
                    var listItem = await _u.Repository<Item>().ListAsynccheck(Itemspec);
                    foreach (var models in listItem)
                    {
                        var specModel = new BaseSpecification<Item>(x => x.ItemId == models.ItemId).AddInclude(x => x.Include(x => x.Bids).Include(x => x.Rating));
                        var model = await _u.Repository<Item>().FindOne(specModel);
                       
                    }
                    return (listItem, Count);
                }
                else if (search == null && cate != null)
                {
                    var Itemspec = new BaseSpecification<Item>(x => x.ItemId == cate).ApplyPaging(skip, take);
                    var listItem = await _u.Repository<Item>().ListAsynccheck(Itemspec);
                    foreach (var models in listItem)
                    {
                        var specModel = new BaseSpecification<Item>(x => x.ItemId == models.ItemId).AddInclude(x => x.Include(x => x.Bids));
                        var model = await _u.Repository<Item>().FindOne(specModel);
                       
                    }
                    return (listItem, Count);
                }
                else if (search == null && cate == null)
                {
                    var Itemspec = new BaseSpecification<Item>().ApplyPaging(skip, take);
                    var listItem = await _u.Repository<Item>().ListAllAsync();
                    foreach (var models in listItem)
                    {
                        var specModel = new BaseSpecification<Item>(x => x.ItemId == models.ItemId).AddInclude(x => x.Include(x => x.Bids).Include(x => x.Rating));
                        var model = await _u.Repository<Item>().FindOne(specModel);
                       
                    }
                    return (listItem, Count);
                }
                else
                {
                    return (null, 0);
                }

            }
            catch (Exception ex)
            {
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
                var itemspec = new BaseSpecification<Item>(x => x.ItemId == id).AddInclude(x => x.Include(x => x.Bids));
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
        public async Task<int> sellItem(Item item)
        {

            try
            {
                var checkName = new BaseSpecification<Item>(x => x.Title == item.Title);
                if (checkName != null)
                {
                    return 0;
                }
                else
                {
                    var additem = await _u.Repository<Item>().AddAsync(item);
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
        public async Task<bool> Ratting(string username, int id, float Rating)
        {
            try
            {
                var Userspec = new BaseSpecification<User>(x => x.Name == username);
                var user = await _u.Repository<User>().FindOne(Userspec);
                var itemspec = new BaseSpecification<Item>(x => x.ItemId == id);
                var item = await _u.Repository<Item>().FindOne(itemspec);
                if (user == null && item == null)
                {
                    return false;
                }
                else
                {
                    var Ratting = new Rating();
                    Ratting.RatingDate = DateTime.Now;
                    Ratting.Rate = Rating;
                    Ratting.ItemId = id;
                    Ratting.RatedUserId = user.UserId;
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
