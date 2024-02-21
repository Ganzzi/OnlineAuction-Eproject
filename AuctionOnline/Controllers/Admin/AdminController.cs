using Application.Interface;
using DomainLayer.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionOnline.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _a;
        public AdminController(IAdminService a)
        {
            _a = a;
        }


        [Route("GetListUserWithRatingAndBidCount")]
        [HttpGet]
        public async Task<IActionResult> GetListUserWithRatingAndBidCount(
            [FromQuery] int page = 1,
            [FromQuery] int take = 10
        )
        {
            var listUser = await _a.ListAllUserWithRatingAndBidCount(take, page);

            var userData = listUser.Item2.Select(x => new
            {
                user = x.User,
                ratings = x.Ratings,
                avgRate = x.AvgRate,
                bidCount = x.BidCount
            }).ToArray();

            var count = listUser.Item1;

            return Ok(new { userData, count });
        }
        
        [Route("LockUnlockUser/{userId}")]
        [HttpPost]
        public async Task<IActionResult> LockUnlockUser(int userId)
        {
            var checkstatus = await _a.LockOrUnlockUser(userId);
            if (checkstatus == true)
            {
                return Ok(new
                {
                    messsage = "Change Status Success"
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Fail to change Status"
                });
            }
        }

        [Route("GetAllCategory")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var listCategory = await _a.ListAllCategoryAndCountItem();
            var categoryList = listCategory.Select(x => new
            {
                Category = x.Category,
                ItemCount = x.ItemCount
            }).ToArray();

            return Ok(new { categorylist = categoryList });
        }

        [Route("CategoryDetailWithListCategoryItems/{CategoryId}")]
        [HttpGet]
        public async Task<IActionResult> CategoryDetailWithListCategoryItems(
            int CategoryId,
            [FromQuery] int page = 1,
            [FromQuery] int take = 10,
            [FromQuery] string? search = "",
            [FromQuery] bool? belongToCategory = null
        )
        {
            var result = await _a.CategoryWithListItem(CategoryId, page, take, search ?? "", belongToCategory);

            if (result != null && result.Category != null)
            {
                var items = result.Items.Select(x => new
                {
                    Item = x.Item,
                    Belong = x.BelongsToCategory
                }).ToList();

                return Ok(new
                {
                    Category = result.Category,
                    Items = items,
                    Count = result.Count
                });
            }
            else
            {
                return NotFound(new
                {
                    message = "Fail Actions"
                });
            }
        }


        [Route("AddCategory")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addcate = await _a.CreateCategory(category);
            if (addcate == true)
            {
                return Ok(new
                {
                    message = "add Success"
                });
            }
            else
            {
                return BadRequest(new
                {
                    mesage = "Invalid category"
                });
            }
        }

        // update cate
        [Route("UpdateCategory/{CategoryId}")]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int CategoryId, [FromBody] Category category)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            category.CategoryId = CategoryId;
            var checkresult = await _a.UpdateCategory(category);
            if (checkresult == true)
            {
                return Ok(new
                {
                    message = "success update"
                });
            }
            else
            {
                return BadRequest("fail update");
            }
        }

        // add+remove item in cate
        [Route("AddDeleteCategoryItem/{CategoryId}/{ItemId}")]
        [HttpPost]
        public async Task<IActionResult> AddDeleteCategoryItem(int CategoryId, int ItemId)
        {
            var checkAddorDel = await _a.AddOrDeleteCategoryItem(CategoryId, ItemId);
            if (checkAddorDel == true)
            {
                return Ok(new
                {
                    message = "Success Action"
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Fail Action"
                });
            }
        }

        [Route("GetListItems")]
        [HttpGet]
        public async Task<IActionResult> GetListItems(
            [FromQuery] int page = 1,
            [FromQuery] int take = 10
        )
        {
            var listItemResult = await _a.ListItemWithCount(page, take);
            if (listItemResult.Item1 != null)
            {
                var listItem = listItemResult.Item1;
                var countPage = listItemResult.Item2;

                var itemList = listItem.Select(x => new
                {
                    Item = x.Item,
                    Categories = x.CategoryCount,
                    BidCount = x.BidCount
                });

                return Ok(new
                {
                    listItem = itemList,
                    Countpage = countPage
                });
            }
            else
            {
                return NotFound(new
                {
                    message = "Fail Actions"
                });
            }
        }

        [Route("ItemDetailWithListCategoryItems/{ItemId}")]
        [HttpGet]
        public async Task<IActionResult> ItemDetailWithListCategoryItems(int ItemId)
        {
            var itemWithCategory = await _a.ItemWithListCategory(ItemId);

            if (itemWithCategory.Item != null)
            {
                var categories = itemWithCategory.Categories.Select(category =>
                    new
                    {
                        Category = category.Category,
                        Belong = category.BelongsToItem // Use the actual property name
                    }).ToList();

                return Ok(new
                {
                    Item = itemWithCategory.Item,
                    Categories = categories,
                });
            }

            return NotFound();
        }
    }
}
