using Application.DTO;
using Application.Interface;
using AuctionOnline.SignalRHub;
using DomainLayer.Core;
using DomainLayer.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AuctionOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtService _j;
        private readonly IuserService _s;
        private readonly IresetEmailService _e;
        private readonly IHubContext<AuctionHub> _hubContext;
        public UserController(IJwtService j, IuserService s, IresetEmailService e, IHubContext<AuctionHub> hubContext)
        {
            _j = j;
            _s = s;
            _e = e;
            _hubContext = hubContext;
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
        public async Task<IActionResult> ListItemsWithQuery(
            [FromQuery] int page = 1,
            [FromQuery] int take = 10, 
            [FromQuery] string? search = "",
            [FromQuery] string? order = "",
            [FromQuery] int? cate = null
        )
        {
            var listSeach = await _s.searchItem(page, take, search, order, cate);
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SellItem([FromForm] SellItemReqest req)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var user = await _s.getUser(_j.dataFormToken(token));

            req.Item.SellerId = user.UserId;

            var sellitem = await _s.sellItem(req);

            Timer timer = null;

            if (sellitem.Item1 != null)
            {
                timer = new Timer(async _ =>
                {
                    if (DateTime.UtcNow >= sellitem.Item1.EndDate)
                    {
                        // TODO: insert notification rows
                        var sendNotification = await _s.AuctionEnd(sellitem.Item1.ItemId);

                        await _hubContext.Clients.Group($"item_{req.Item.ItemId}")
                            .SendAsync("AuctionEnded", sellitem.Item1.ItemId, sellitem.Item1.SellerId);
                        timer.Dispose(); 
                    }
                }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1)); 

                return Ok(new
                {
                    message = sellitem.Item2,
                });
            }
            else {
                return BadRequest(new
                {
                    message = sellitem.Item2,
                });
            }
        }

        //Update item
        // TODO: finish function, delete old & add CategoryItem[] to db base on category[]
        // ****checked
        [Route("UpdateItem")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateItem([FromForm] SellItemReqest req)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var user = await _s.getUser(_j.dataFormToken(token));

            req.Item.SellerId = user.UserId;

            var responforUpdateItem = await _s.updateItem(req);
            if (responforUpdateItem.Item1 == true)
            {
                return Ok(new
                {
                    message = responforUpdateItem.Item2
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = responforUpdateItem.Item2
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
            var user = await _s.getUser(username);

            var auctionHistory = await _s.PlaceABid(req, user);

            if (auctionHistory != null)
            {
                if (auctionHistory.WinnerId == null)
                {
                    return Ok(new
                    {
                        message = "Success Action"
                    });
                }
                if (auctionHistory.WinnerId == user.UserId)
                {

                    //
                    var sendNotification = await _s.AuctionEnd(auctionHistory.ItemId);

                    await _hubContext.Clients.Group($"item_{auctionHistory.Item.ItemId}")
                        .SendAsync("AuctionEnded", auctionHistory.Item.ItemId, auctionHistory.Item.SellerId);
                    return Ok(new
                    {
                        message = "Success Action, You are the winner!"
                    });
                }
                return BadRequest(new
                {
                    message = "Item's winner has been identified"
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Item not exist or error"
                });
            }
        }

        // get profile
        // TODO: get user basic (name, email, role, id,...) info base on token
        [Route("Profile")]
        [HttpGet]
        public async Task<IActionResult> Profile()
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
        [Route("Profiledetail")]
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
        // TODO
        // get auction history detail
        // TODO: get auction history model from userId (use token) & AuctionHistoryId
        // ***checked
        [Route("AuctionHistoryDetail")]
        [HttpGet]
        public async Task<IActionResult> AuctionHistoryDetail(int AuctionHistoryId)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var username = _j.dataFormToken(token);
            var Auction = await _s.GetAcutionHistory(username, AuctionHistoryId);
            if (Auction != null)
            {
                return Ok(Auction);
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
        // TODO
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
        public async Task<IActionResult> UpdateUser([FromForm] User user)
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

        // TODO
        //checkEmail and send link reset
        [Route("sendlink")]
        [HttpPost]
        public async Task<IActionResult> checkemailandsendlink(string email)
        {
            var checkEmail = await _e.CheckEmailAndTokenEmail(email);
            if (checkEmail == null)
            {
                return BadRequest(new
                {
                    message = "No Email exit"
                });
            }
            if (_e.sendMail(checkEmail))
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
                    message = "Fail to send Reset link"
                });
            }
        }

        // TODO
        //reset Email
        [Route("resetpassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var checkPasswordReset = await _e.checkTokenEmailAndSaveNewPassword(model);
            if (checkPasswordReset == 0)
            {
                return BadRequest(new
                {
                    message = "Fail Action"
                });
            }
            else if (checkPasswordReset == 1)
            {
                return Ok(new
                {
                    message = "Success Action"
                });
            }
            else if (checkPasswordReset == -1)
            {
                return BadRequest(new
                {
                    message = "Token Expire Or invalid Token"
                });
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
