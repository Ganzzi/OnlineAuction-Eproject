﻿using Application.Interface;
using DomainLayer.Core;
using DomainLayer.Entities.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Route("CategoriesWithTenItems")]
        [HttpGet]
        public async Task<IActionResult> CategoriesWithTenItems()
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
        [Route("GetItemById")]
        [HttpGet]
        public async Task<IActionResult> GetItemById(int id)
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
        [Route("ListItemsWithQuery")]
        [HttpGet]
        public async Task<IActionResult> ListItemsWithQuery(int page, int take, string? search, int? cate)
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
        // TODO: add CategoryItem[] to db base on category[]
        // ****checked
        [Route("SellItem")]
        [HttpPost]
        public async Task<IActionResult> SellItem([FromBody] SellItemReqest req)
        {
            var sellitem = await _s.sellItem(req);
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

        //Update item
        // TODO: finish function, delete old & add CategoryItem[] to db base on category[]
        // ****checked
        [Route("UpdateItem")]
        [HttpPost]
        public async Task<IActionResult> UpdateItem([FromBody] SellItemReqest req)
        {
            var responforUpdateItem = await _s.updateItem(req);
            if (responforUpdateItem == true)
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
                    message = "Fail Actions"
                });
            }
        }

        // Place a bid
        // TODO: finish function, get userid by token, new Bid model
        [Route("PlaceBid")]
        [HttpPost]
        public async Task<IActionResult> PlaceBid([FromBody] PlaceBidRequest req)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var username = _j.dataFormToken(token);
            var bidCheck = await _s.placenewbid(req, username);
            if (bidCheck == true)
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

        // get profile
        // TODO: get user basic (name, email, role, id,...) info base on token
        [Route("Profile")]
        [HttpGet]
        public async Task<IActionResult> userProfile()
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var username = _j.dataFormToken(token);
            var getuser = await _s.getUser(username);
            if (getuser != null)
            {
                return Ok(new { User = getuser });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Fail Action"
                });
            }
        }

        // get profile
        // TODO: include solditems, bids, auction history (user model)
        //***check
        [Route("Profiledetails")]
        [HttpGet]
        public async Task<IActionResult> ProfileDetail()
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var username = _j.dataFormToken(token);
            var getUser = await _s.getProfileDetail(username);
            if (getUser != (null, 0))
            {
                return Ok(new
                {
                    user = getUser.Item1,
                    countitem = getUser.Item2
                });
            }
            return Ok();
        }

        // get auction history detail
        // TODO: get auction history model from userId (use token) & AuctionHistoryId
        // ***checked
        [Route("AuctionHistoryDetail")]
        [HttpGet]
        public async Task<IActionResult> AuctionHistoryDetail(int AuctionHistoryId)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var getAuction = await _s.GetAcutionHistory(token, AuctionHistoryId);
            if (getAuction != null)
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

        //rating 
        // TODO: 
        // - new rating model, raterId: userId from token, ratedUserId, amount from request
        // - test function
        //
        [Route("RateBuyer")]
        [HttpPost]
        public async Task<IActionResult> RateBuyer([FromBody] RateBuyerRequest req)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var username = _j.dataFormToken(token);
            var RateAction = await _s.Ratting(username, req);
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

        //update user
        // TODO: handle avatar file
        // *** checked
        [Route("UpdateUser")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
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

    }
}
