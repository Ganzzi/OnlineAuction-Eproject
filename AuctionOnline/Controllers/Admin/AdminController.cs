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
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServicevice _a;
        public AdminController(IAdminServicevice a)
        {
            _a = a;
        }

        [Route("getall")]
        [HttpGet]
        public async Task<IActionResult> getallUser([FromQuery] int page = 1,
        [FromQuery] int take = 10)
        {

            var listUser = await _a.ListAllUser(take, page);

            return Ok(listUser.ToString());
        }

        // TODO: additionally return average rated amount ***checked
        //all user
        [Route("getallUser")]
        [HttpGet]
        public async Task<IActionResult> getall([FromQuery] int page = 1,
         [FromQuery] int take = 10)
        {

            var listUser = await _a.ListAllUserWithRatingAndBidCount(take, page);

            return Ok(new
            {
                userData = listUser.Select(x => new
                {
                    userName = x.Key,
                    avgRating = x.Value.Item1,
                    bidCount = x.Value.Item2
                }).ToArray()
            });
        }

        //all cate
        [Route("getallCategory")]
        [HttpGet]
        public async Task<IActionResult> getallCategory()
        {
            var listcate = await _a.ListAllCategoryAndCountItem();
            return Ok(new
            {
                categorylist = listcate.Select(x => new
                {
                    categoryName = x.Key,
                    ItemCount = x.Value
                }).ToArray()
            });
        }

        //lock user
        [Route("lock/{name}/{status}")]
        [HttpPost]
        public async Task<IActionResult> lockUserAction(string name, string status)
        {
            var checkstatus = await _a.LockOrUnlock(name, status);
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

        //add cate
        [Route("addCategory")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
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
        [Route("UpdateCategory")]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            if (category.CategoryName == null || category.Description == null)
            {
                return BadRequest(new
                {
                    message = "Invalid category"
                });
            }
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

        // get item by id
        [Route("getItem/{id}")]
        [HttpGet]
        public async Task<IActionResult> getItem(int id)
        {
            var item = await _a.takeOneItem(id);
            if (item == null)
            {
                return NotFound(new
                {
                    message = "item may not exit"
                });
            }
            else
            {
                return Ok(item);
            }
        }

        // add+remove item in cate
        [Route("ItemInCate")]
        [HttpPost]
        public async Task<IActionResult> AddorDelItemInCate(bool status, int cate, int item)
        {
            var checkAddorDel = await _a.addOrDeleteItemForCate(cate, item, status);
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

        [Route("listItem")]
        [HttpGet]
        public async Task<IActionResult> ListItemInAdmin(int page, int take)
        {
            var listItem = await _a.getListItemhaveCount(page, take);
            if (listItem != (null, 0))
            {
                return Ok(new
                {
                    listItem = listItem.Item1.Select(x => new
                    {
                        itemName = x.Key,
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

        [Route("ItemWithListCategoryItem")]
        [HttpGet]
        public async Task<IActionResult> ItemAndListCategoryItem(int id, int page, int take)
        {
            var listItem = await _a.GetOneItemAndListCategoryItem(id, page, take);
            if (listItem != (null, null, 0))
            {
                return Ok(new
                {
                    Item = listItem.Item1,
                    listcategoryItem = listItem.Item2,
                    Count = listItem.Item3
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
