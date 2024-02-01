using Application.DTO;
using Application.Interface;
using Application.Service;
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
        private readonly IresetEmailService _e;
        public AuthController(IAuthService authService, IJwtService jwt)
        {
            _authService = authService;
            _jwt = jwt; 
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            if (!IsValidPassword(model.Password))
            {
                return BadRequest(new
                {
                    message = "Invalid Password"
                });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hashpassword = _authService.HashPassWord(model.Password);
            model.Password = hashpassword;
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

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel model)
        {
            if (!IsValidPassword(model.Password))
            {
                return BadRequest(new
                {
                    message = "Invalid Password"
                });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var hashpassword = _authService.HashPassWord(model.Password);
            model.Password = hashpassword;
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
        [NonAction]
        public bool IsValidPassword(string password)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
            return regex.IsMatch(password);
        }
    }
}
