using Application.DTO;
using Application.Interface;
using Application.Service.AdminServicevice;
using DomainLayer.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuctionOnline.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServicevice _a;
        public AdminController(IAdminServicevice a)
        {
            _a = a;
        }

        [Route("GetListUser")]
        [HttpGet]
        public async Task<IActionResult> GetListUser(
            [FromQuery] int page = 1,
            [FromQuery] int take = 10
        )
        {

            var listUser = await _a.ListAllUserWithRatingAndBidCount(take, page);

            return Ok(new
            {
                userData = listUser.Select(x => new
                {
                    user = x.Key,
                    avgRating = x.Value.Item1,
                    bidCount = x.Value.Item2
                }).ToArray()
            });
        }

        [Route("LockUnlockUser/{userId}")]
        [HttpPost]
        public async Task<IActionResult> LockUnlockUser(int userId)
        {
            var checkstatus = await _a.LockOrUnlock(userId);
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
            var listcate = await _a.ListAllCategoryAndCountItem();
            return Ok(new
            {
                categorylist = listcate.Select(x => new
                {
                    Category = x.Key,
                    ItemCount = x.Value
                }).ToArray()
            });
        }

        [Route("CategoryDetailWithListCategoryItems/{CategoryId}")]
        [HttpGet]
        public async Task<IActionResult> CategoryDetailWithListCategoryItems(
            int CategoryId,
            [FromQuery] int page = 1,
            [FromQuery] int take = 10, 
            [FromQuery] string? search = "",
            [FromQuery] bool? belongtocategory = null
        )
        {
            var Item = await _a.CategorylistItem(CategoryId, page, take, search, belongtocategory);
            if (Item.Item1 != null)
            {
                return Ok(new
                {
                    Item = Item.Item1,
                    CategoryItems = Item.Item2,
                    Count = Item.Item3
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
            var checkAddorDel = await _a.addOrDeleteItemForCate(CategoryId, ItemId);
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
            var listItem = await _a.getListItemhaveCount(page, take);
            if (listItem != (null, 0))
            {
                return Ok(new
                {
                    listItem = listItem.Item1.Select(x => new
                    {
                        Item = x.Key,
                        AvgRate = x.Value.Item1,
                        bidCount = x.Value.Item2,
                    }),
                    Countpage = listItem.Item2
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
            var Item = await _a.GetOneItemAndListCategoryItem(ItemId);
            if (Item.Item1 != null)
            {
                return Ok(new
                {
                    Item = Item.Item1,
                    CategoryItems = Item.Item2,
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
    }


}
