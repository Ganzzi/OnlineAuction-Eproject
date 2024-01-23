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
    internal class UserService : IuserService
    {
        private readonly IUnitOfWork _u;
        private readonly IphotoService _p;
        public UserService(IUnitOfWork u, IphotoService p)
        {
            _u = u;
            _p = p;
        }

        // categorylist 
        public async Task<IList<Category>> categorylist()
        {
            try
            {
                var categoryspec = new BaseSpecification<Category>()
                     .AddInclude(x => x.Include(x => x.CategoryItems).ThenInclude(x => x.Item));
                var liscategory = await _u.Repository<Category>().ListAsynccheck(categoryspec);

                // foreach (var item in liscategory)
                // {
                //     var categoryItemSpec = new BaseSpecification<CategoryItem>(x => x.CategoryId == item.CategoryId)
                //         .AddInclude(x => x.Include(x => x.Item).ThenInclude(x => x.Bids));
                //     var categoryItemList = await _u.Repository<CategoryItem>().FindOne(categoryItemSpec);
                //     // var TopTen = categoryItemList.Item.Bids.OrderByDescending(x => x.BidAmount).Take(10);
                // }
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

        // update User => img
        public async Task<User> UpdateUser(User model)
        {
            try
            {
                var Userspec = new BaseSpecification<User>(x => x.Name == model.Name);
                var user = await _u.Repository<User>().FindOne(Userspec);
                if (user != null && user.AvatarFile != null)
                {
                    var deleteCloudinary = await _p.DeletPhoto(user.Avatar);
                    var CloudinaryUserAvatar = await _p.addPhoto(user.AvatarFile);
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.Password = model.Password;
                    user.Avatar = CloudinaryUserAvatar;
                    _u.Repository<User>().Update(user);
                    await _u.SaveChangesAsync();
                    return user;
                }
                else if (user != null && user.AvatarFile == null)
                {
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.Password = model.Password;
                    _u.Repository<User>().Update(user);
                    await _u.SaveChangesAsync();
                    return user;
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
                    itemSpec = new BaseSpecification<CategoryItem>(x => x.Item.Title.Contains(search));
                }
                else if (cate.HasValue)
                {
                    itemSpec = new BaseSpecification<CategoryItem>(x => x.CategoryId == cate.Value);
                }
                else
                {
                    itemSpec = new BaseSpecification<CategoryItem>();
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
                    .AddInclude(x => x.Include(x => x.Bids).Include(x => x.AuctionHistory).Include(x => x.CategoryItems).ThenInclude(ci => ci.Category).Include(x => x.Seller));
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

        // sell item => img
        public async Task<int> sellItem(SellItemReqest req)
        {
            try
            {
                // Check if an item with the same title already exists
                var existingItem = await _u.Repository<Item>().FindOne(new BaseSpecification<Item>(x => x.Title == req.Item.Title));
                if (existingItem != null)
                {
                    // Item with the same title already exists
                    return 0;
                }

                // Add the item with the associated image
                req.Item.Image = await _p.addPhoto(req.Item.ImageFile);
                var addedItem = await _u.Repository<Item>().AddAsync(req.Item);

                await _u.SaveChangesAsync();

                // Check if the item was added successfully
                if (addedItem == null || addedItem.ItemId <= 0)
                {
                    // Handle the case where the item was not added successfully
                    return -1;
                }

                // Create CategoryItem entities for each category and associate them with the added item
                foreach (var re in req.Categories)
                {
                    var cateItem = new CategoryItem();
                    cateItem.ItemId = addedItem.ItemId; // Make sure ItemId is valid
                    cateItem.CategoryId = re.CategoryId;
                    var addcateItem = await _u.Repository<CategoryItem>().AddAsync(cateItem);
                }

                await _u.Repository<AuctionHistory>().AddAsync(new AuctionHistory
                    {
                        ItemId = addedItem.ItemId,
                        EndDate = new DateTime(),
                        WinningBid = 0
                    });

                // Save changes
                await _u.SaveChangesAsync();

                // Return success status
                return 1;
            }
            catch (Exception ex)
            {
                // Handle exceptions and roll back changes
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
                if (user == null || item == null)
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
                    await _u.Repository<Rating>().AddAsync(Ratting);

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

        //item update => img
        public async Task<bool> updateItem(SellItemReqest req)
        {
            try
            {
                var checkName = new BaseSpecification<Item>(x => x.Title == req.Item.Title);
                var finditem = await _u.Repository<Item>().FindOne(checkName);
                var specCategoryItem = new BaseSpecification<CategoryItem>(x => x.ItemId == finditem.ItemId);
                var cateItem = await _u.Repository<CategoryItem>().FindOne(specCategoryItem);
                if (req.Item.ImageFile == null)
                {
                    // dont have img
                    _u.Repository<CategoryItem>().Delete(cateItem);
                    var newItem = new Item();
                    newItem.Title = req.Item.Title;
                    newItem.Description = req.Item.Description;
                    newItem.ReservePrice = req.Item.ReservePrice;
                    newItem.StartingPrice = req.Item.StartingPrice;
                    newItem.IncreasingAmount = req.Item.IncreasingAmount;
                    newItem.StartDate = req.Item.StartDate;
                    newItem.EndDate = req.Item.EndDate;
                    newItem.Image = finditem.Image;
                    await _u.Repository<Item>().AddAsync(newItem);
                    foreach (var item in req.Categories)
                    {
                        var newcateItem = new CategoryItem();
                        newcateItem.ItemId = finditem.ItemId;
                        newcateItem.CategoryId = item.CategoryId;
                        await _u.Repository<CategoryItem>().AddAsync(newcateItem);
                    }
                    await _u.SaveChangesAsync();
                    return true;
                }
                else if (req.Item.ImageFile != null)
                {
                    // have img
                    _u.Repository<CategoryItem>().Delete(cateItem);
                    await _p.DeletPhoto(finditem.Image);
                    var addpicture = await _p.addPhoto(req.Item.ImageFile);
                    var newItem = new Item();
                    newItem.Title = req.Item.Title;
                    newItem.Description = req.Item.Description;
                    newItem.ReservePrice = req.Item.ReservePrice;
                    newItem.StartingPrice = req.Item.StartingPrice;
                    newItem.IncreasingAmount = req.Item.IncreasingAmount;
                    newItem.StartDate = req.Item.StartDate;
                    newItem.EndDate = req.Item.EndDate;
                    newItem.Image = addpicture;
                    await _u.Repository<Item>().AddAsync(newItem);


                    foreach (var item in req.Categories)
                    {
                        var newcateItem = new CategoryItem();
                        newcateItem.ItemId = finditem.ItemId;
                        newcateItem.CategoryId = item.CategoryId;
                        await _u.Repository<CategoryItem>().AddAsync(newcateItem);
                    }
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

        //get AcutionHistory 
        public async Task<AuctionHistory> GetAcutionHistory(string username, int id)
        {
            try
            {
                var specUser = new BaseSpecification<User>(x => x.Name == username);
                var user = await _u.Repository<User>().FindOne(specUser);
                // var specAuctionHistory = new BaseSpecification<AuctionHistory>
                //     (x => (x.WinnerId == user.UserId || x.Item.SellerId == user.UserId) && x.AuctionHistoryId == id)
                //     .AddInclude(q => q.Include(ah => ah.Winner).Include(ah => ah.Item));

                var specAuctionHistory = new BaseSpecification<AuctionHistory>
                    (x => x.AuctionHistoryId == id && (x.Item.SellerId == user.UserId || (x.WinnerId != null && x.WinnerId == user.UserId)))
                    .AddInclude(q => q.Include(ah => ah.Winner).Include(ah => ah.Item).ThenInclude(i => i.Seller).Include(ah => ah.Item).ThenInclude(i => i.Rating));

                var AuctionHistory = await _u.Repository<AuctionHistory>().FindOne(specAuctionHistory);
                return AuctionHistory;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        // place bid
        public async Task<bool> PlaceABid(PlaceBidRequest req, string username)
        {
            try
            {
                var specUser = new BaseSpecification<User>(x => x.Name == username);
                var user = await _u.Repository<User>().FindOne(specUser);
                var Item = await _u.Repository<Item>().FindOne(new BaseSpecification<Item>(i => i.ItemId == req.ItemId));
                var ah = await _u.Repository<AuctionHistory>().FindOne(new BaseSpecification<AuctionHistory>(ah => ah.ItemId == Item.ItemId));
                
                if (ah != null && ah.WinnerId != null)    
                {
                    return false;
                }

                if (user != null && Item != null)
                {
                    var specBid = new BaseSpecification<Bid>(x => x.ItemId == req.ItemId);
                    var bid = new Bid();
                    bid.UserId = user.UserId;
                    bid.ItemId = req.ItemId;
                    bid.BidAmount = req.Amount;
                    await _u.Repository<Bid>().AddAsync(bid);

                    if (req.Amount >= Item.ReservePrice)
                    {
                        ah.WinnerId = user.UserId;
                        _u.Repository<AuctionHistory>().Update(ah);
                    }

                    await _u.SaveChangesAsync();
                    return true;
                }
                else { return false; }
            }
            catch (Exception e)
            {
                await _u.RollBackChangesAsync();
                return false;
            }

        }

        //Profile details
        public async Task<(User, int)> getProfileDetail(string username)
        {
            try
            {
                var specUser = new BaseSpecification<User>(x => x.Name == username).AddInclude(x => x.Include(x => x.SoldItems).ThenInclude(i => i.AuctionHistory).Include(c => c.Bids).ThenInclude(b => b.Item).Include(i => i.AuctionHistories).ThenInclude(ah => ah.Item));
                var user = await _u.Repository<User>().FindOne(specUser);
                int itemCount = user.SoldItems.Count();
                if (user != null)
                {
                    return (user, itemCount);
                }
                else
                {
                    return (null, 0);
                }
            }
            catch (Exception e)
            {
                return (null, 0);
            }

        }
    }
}
