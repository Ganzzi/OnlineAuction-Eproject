using Application.Interface;
using DomainLayer.Core;
using DomainLayer.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtService _j;
        private readonly IuserService _s;
        public UserController(IJwtService j, IuserService s)
        {
            _j = j;
            _s = s;
        }



        // categorylist 
        [Route("getTopTen")]
        [HttpGet]
        public async Task<IActionResult> categoryListwithTopTenItem()
        {
            var topTen = await _s.categorylist();
            if (topTen != null)
            {
                return Ok(topTen);
            }
            else
            {
                return BadRequest(new
                {
                    message = "Fail Actions"
                });
            }
        }

        // itembyid
        [Route("getItem")]
        [HttpGet]
        public async Task<IActionResult> getItemUseId(int id)
        {
            var item = await _s.getItemById(id);
            if (item != null)
            {
                return Ok(item);
            }
            else
            {
                return NotFound(new
                {
                    message = "Fail Actions"
                });
            }
        }

        // search item
        [Route("searchItem")]
        [HttpGet]
        public async Task<IActionResult> searchItemList(int page, int take, string? search, int? cate)
        {
            var listSeach = await _s.searchItem(page, take, search, cate);
            if (listSeach == (null, 0))
            {
                return NotFound();
            }
            else
            {
                return Ok(new
                {
                    listSearch = listSeach.Item1,
                    Count = listSeach.Item2
                });
            }
        }

        //sell item
        [Route("sell")]
        [HttpPost]
        public async Task<IActionResult> Sell([FromBody] Item item)
        {
            var sellitem = await _s.sellItem(item);
            if (sellitem == 1)
            {
                return Ok(new
                {
                    message = "success actions"
                });
            }
            else if (sellitem == 0)
            {
                return BadRequest(new
                {
                    message = "Title existed please chose another title"
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

        //update user
        [Route("updateUser")]
        [HttpPost]
        public async Task<IActionResult> updateUser([FromBody] User user)
        {
            var userupdate = await _s.UpdateUser(user);
            if (userupdate != null)
            {
                return Ok(new
                {
                    message = "success update"
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Fail Actions"
                });
            }
        }

        //rating 
        [Route("Ratting")]
        [HttpPost]
        public async Task<IActionResult> RateItem(int id, float rate)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var username = _j.dataFormToken(token);
            var RateAction = await _s.Ratting(username, id, rate);
            if (RateAction == false)
            {
                return BadRequest(new
                {
                    message = "Fail Action"
                });
            }
            else
            {
                return Ok(new
                {
                    message = "success"
                });
            }

        }
    }
}
