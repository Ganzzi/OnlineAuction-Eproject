using Application.DTO;
using Application.Interface;
using Application.Service;
using Azure.Core;
using DomainLayer.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace AuctionOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwt;
        public AuthController(IAuthService authService, IJwtService jwt)
        {
            _authService = authService;
            _jwt = jwt; 
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            //if (!IsValidPassword(model.Password))
            //{
            //    return BadRequest(new
            //    {
            //        message = "Invalid Password"
            //    });
            //}
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var User = await _authService.Login(model);
            if (User == null)
            {
                return Unauthorized();
            }
            var token = await _jwt.CreateToken(User);
            var RefreshToken = await  _jwt.createRrefreshtoken(User.UserId);
            
            return Ok(new
            {
               AccessToken = token,
               RefreshToken = RefreshToken.Token,
            }) ;
        }

        [Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user2 = await _authService.Register(model);
            if (user2 != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("RefreshToken")]
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest req)
        {
            var validtoken = await _jwt.RefreshAccessToken(req.AccessToken);
            if (validtoken == null)
            {
                return BadRequest(new
                {
                    message = "Invalid Token"
                });
            }
            else
            {
                return Ok(new {
                    AccessToken = validtoken.AccessToken
                });
            }
        }


        [NonAction]
        public bool IsValidPassword(string password)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
            return regex.IsMatch(password);
        }
    }
}
