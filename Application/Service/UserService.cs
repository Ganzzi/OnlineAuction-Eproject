using Application.Interface;
using DomainLayer.Core;
using DomainLayer.Entities.Models;
using DomainLayer.SpecificationPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly RedisService _redisService;
        public UserService(IUnitOfWork u, IphotoService p, RedisService redisService)
        {
            _u = u;
            _p = p;
            _redisService = redisService;
        }

        // categorylist 
        public async Task<IList<Category>> categorylist()
        {
            try
            {
                var value = await _redisService.GetCachedData<Category[]>("list_category");

                if (value.IsNullOrEmpty())
                {
                    var categoryspec = new BaseSpecification<Category>();
                        //  .AddInclude(x => x.Include(x => x.CategoryItems).ThenInclude(x => x.Item));
                    var liscategory = await _u.Repository<Category>().ListAsynccheck(categoryspec);

                    foreach (var item in liscategory)
                    {
                        var cateItemSpec = new BaseSpecification<CategoryItem>
                            (ci => ci.CategoryId == item.CategoryId)
                            .AddInclude(q => q.Include(ci => ci.Item))
                            .ApplyPaging(0, 10)
                            .ApplyOrderByDescending(ci => ci.Item.Bids.Count); 
                            item.CategoryItems = await _u.Repository<CategoryItem>().ListAsynccheck(cateItemSpec);
                    }
                    value = liscategory.ToArray();
                    _redisService.SetCachedData<Category[]>("list_category", value, TimeSpan.FromSeconds(200));
                }

                return value.ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }

        //UserProfile ****
        public async Task<User> getUser(string username)
        {
            var spec = new BaseSpecification<User>(x => x.Name == username).AddInclude(x => x.Include(x => x.Notifications));
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
                var Userspec = new BaseSpecification<User>(x => x.UserId == model.UserId);
                var user = await _u.Repository<User>().FindOne(Userspec);

                if (model.AvatarFile != null)
                {
                    if (user.Avatar != null)
                    {
                        await _p.DeletPhoto(user.Avatar);   
                    }
                    var CloudinaryUserAvatar = await _p.addPhoto(model.AvatarFile);
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.Avatar = CloudinaryUserAvatar;
                }
                else 
                {
                    user.Name = model.Name;
                    user.Email = model.Email;
                }
                if (model.Password != null) user.Password = model.Password;
                
                _u.Repository<User>().Update(user);
                await _u.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                await _u.RollBackChangesAsync();
                return null;
            }

        }

        //item list with search query
        public async Task<(IList<Item>, int)> searchItem(int page, int take, string search, string order, int? cate)
        {
            try
            {
                var skip = take * (page - 1);

                var iSpec = new BaseSpecification<Item>(
                    x => (search == null || x.Title.Contains(search)) && (cate == null || x.CategoryItems.Any(ci => ci.CategoryId == cate))
                );
                var count = await _u.Repository<Item>().CountAsync(iSpec);
                 iSpec = iSpec
                            .ApplyPaging(skip, take)
                            .AddInclude(x => x.Include(x => x.Bids))
                            .ApplyOrderBy(x => order == "date" ? x.StartDate : x.Title);
                
                var listItem = await _u.Repository<Item>().ListAsynccheck(iSpec);

                return (listItem, count);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return (null, 0);
            }
        }


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
        public async Task<(Item,string)> sellItem(SellItemReqest req)
        {
            try
            {
                // check valid Price and valid time in req
                if (req.Item.ReservePrice < req.Item.StartingPrice)
                {
                    return (null, "require: ReservePrice > StartingPrice");
                }
                if (req.Item.StartDate > req.Item.EndDate)
                {
                    return (null, "require: StartDate < EndDate");
                }
                if (req.Categories.Count() <= 0)
                {
                    return (null, "require: at least 1 category"); ; ;
                }
              

                // Check if an item with the same title already exists
                var existingItem = await _u.Repository<Item>().FindOne(new BaseSpecification<Item>(x => x.Title == req.Item.Title));
                if (existingItem != null)
                {
                    // Item with the same title already exists
                    return (null,"change title");
                }

                if (req.Item.ImageFile == null)
                {
                    return (null, "require: item image"); ; ;
                }

                // Add the item with the associated image
                req.Item.Image = await _p.addPhoto(req.Item.ImageFile);
                var addedItem = await _u.Repository<Item>().AddAsync(req.Item);

                await _u.SaveChangesAsync();

                // Check if the item was added successfully
                if (addedItem == null || addedItem.ItemId <= 0)
                {
                    // Handle the case where the item was not added successfully
                    return (null,"item not exit");
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
                return (addedItem,"success");
            }
            catch (Exception ex)
            {
                // Handle exceptions and roll back changes
                await _u.RollBackChangesAsync();
                return (null,"FailAction");
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
        public async Task<(bool,string)> updateItem(SellItemReqest req)
        {
            try
            {
                string message = "";
                var checkName = new BaseSpecification<Item>(x => x.ItemId == req.Item.ItemId);
                var finditem = await _u.Repository<Item>().FindOne(checkName);

                // check valid Price and valid time in req
             
                if (req.Item.ReservePrice < req.Item.StartingPrice)
                {
                    return (false, "require: ReservePrice > StartingPrice");
                }
                if (req.Item.IncreasingAmount < req.Item.StartingPrice * 0.1)
                {
                    return (false, "require: IncreasingAmount > 10% StartingPrice");
                }
                if (req.Item.StartDate > req.Item.EndDate)
                {
                    return (false, "require: StartDate < EndDate");
                }
                if (req.Categories.Count() <= 0)
                {
                    return (false, "require: at least 1 category");
                }
                if (req.Item.StartDate < finditem.StartDate)
                {
                    return (false, "require: new StartDate > old StartDate");
                }
                var newItem = finditem;
                newItem.Title = req.Item.Title;
                newItem.Description = req.Item.Description;
                newItem.ReservePrice = req.Item.ReservePrice;
                newItem.StartingPrice = req.Item.StartingPrice;
                newItem.IncreasingAmount = req.Item.IncreasingAmount;
                newItem.StartDate = req.Item.StartDate;
                newItem.EndDate = req.Item.EndDate;
                newItem.SellerId = req.Item.SellerId;

           
                if (req.Item.ImageFile != null)
                {
                    await _p.DeletPhoto(finditem.Image);
                    var addpicture = await _p.addPhoto(req.Item.ImageFile);
                    newItem.Image = addpicture; 
                }

                _u.Repository<Item>().Update(newItem);
                await _u.SaveChangesAsync();


                var specCategoryItem = new BaseSpecification<CategoryItem>(x => x.ItemId == finditem.ItemId).AddInclude(q => q.Include(ci => ci.Category));
                var cateItem = await _u.Repository<CategoryItem>().ListAsynccheck(specCategoryItem);

                List<Category> oldList = cateItem.Select(item => item.Category).ToList();
                List<Category> newList = req.Categories.ToList();

                var comparer = new CategoryComparer();

                var categoriesToAdd = newList.Where(newCategory => !oldList.Any(oldCategory => comparer.Equals(oldCategory, newCategory))).ToList();
                var categoriesToRemove = oldList.Where(oldCategory => !newList.Any(newCategory => comparer.Equals(oldCategory, newCategory))).ToList();

                foreach (var item in categoriesToAdd)
                {
                    await _u.Repository<CategoryItem>().AddAsync(new CategoryItem {
                        ItemId = finditem.ItemId,
                        CategoryId = item.CategoryId
                    });
                }

                foreach (var item in categoriesToRemove)
                {
                    var spec = new BaseSpecification<CategoryItem>(x => x.CategoryId == item.CategoryId);
                    var f = await _u.Repository<CategoryItem>().FindOne(spec);
                    _u.Repository<CategoryItem>().Delete(f);
                }

                await _u.SaveChangesAsync();

                return (true, "Update Item Success");
            }
            catch (Exception e)
            {
                await _u.RollBackChangesAsync();
                return (false, e.Message);
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
        public async Task<AuctionHistory> PlaceABid(PlaceBidRequest req, User user)
        {
            try
            {
                var Item = await _u.Repository<Item>().FindOne(new BaseSpecification<Item>(i => i.ItemId == req.ItemId));
                
                if (Item == null)
                {
                    return null;
                }

                var ah = await _u.Repository<AuctionHistory>()
                    .FindOne(new BaseSpecification<AuctionHistory>(ah => ah.ItemId == Item.ItemId));
                
                if (ah.WinnerId != null)    
                {
                    return ah;
                }

                var specBid = new BaseSpecification<Bid>(x => x.ItemId == req.ItemId);
                var bid = new Bid();
                bid.UserId = user.UserId;
                bid.ItemId = req.ItemId;
                bid.BidAmount = req.Amount;
                await _u.Repository<Bid>().AddAsync(bid);

                if (req.Amount >= Item.ReservePrice)
                {
                    // TODO: insert notification rows                  
                    ah.WinnerId = user.UserId;                  
                    _u.Repository<AuctionHistory>().Update(ah);
                }

                
                await _u.SaveChangesAsync();
                return ah;
            }
            catch (Exception e)
            {
                await _u.RollBackChangesAsync();
                return null;
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

        // notification
        public async Task<bool> AuctionEnd(int ItemId)
        {
            try
            {
                // auction history => winner => userid       
                var specAuct = new BaseSpecification<AuctionHistory>(c => c.ItemId == ItemId).AddInclude(x => x.Include(x => x.Item).Include(x => x.Winner));
                var Auct = await _u.Repository<AuctionHistory>().FindOne(specAuct);


                // seller
                var sellerNoti = new Notification
                {
                    ItemId = ItemId,
                    UserId = Auct.Item.SellerId,
                    NotificationContent = "your item has been sold",
                    NotificationDate = DateTime.Now,
                };
                    
                // winner
                var winnerNoti = new Notification
                {
                    ItemId = ItemId,
                    UserId = Auct.Winner.UserId,
                    NotificationContent = "your are the winner",
                    NotificationDate = DateTime.Now,
                };
                await _u.Repository<Notification>().AddAsync(sellerNoti);
                await _u.Repository<Notification>().AddAsync(winnerNoti);
                await _u.SaveChangesAsync();

                return true; 
            }
            catch (Exception e) { 
              await  _u.RollBackChangesAsync();
                return false;
            }
           
        }
    }
}

public class CategoryComparer : IEqualityComparer<Category>
{
    public bool Equals(Category x, Category y)
    {
        // Implement logic to compare categories, e.g., based on CategoryId
        return x.CategoryId == y.CategoryId;
    }

    public int GetHashCode(Category obj)
    {
        // Implement logic to generate a hash code for a category
        return obj.CategoryId.GetHashCode();
    }
}