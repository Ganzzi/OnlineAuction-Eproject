using Application.DTO;
using Application.Interface;
using Application.Service;
using DomainLayer.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace AcutionOnline.Controllers
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

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> signIn(LoginModel model)
        {
            //if (!IsValidPassword(model.Password))
            //{
            //    return BadRequest(new
            //    {
            //        message = "Invalid Password"
            //    });
            //}
            var User = await _authService.Login(model);
            if (User == null)
            {

                return NotFound();
            }
            var token = _jwt.CreateToken(User);
            var RToken = _jwt.createRrefreshtoken(User.UserId);
            
            return Ok(new
            {
               AcessToken = token.Result,
               RToken = RToken.Result.Token,
            }) ;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterModel model)
        {
            var user2 = await _authService.Register(model);
            if (user2 != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("checkToken")]
        [HttpPost]
        public async Task<IActionResult> checkvalidToken(string token)
        {
            var validtoken = await _jwt.checkToken(token);
            if (validtoken == null)
            {
                return BadRequest(new
                {
                    message = "Invalid Token"
                });
            }
            else
            {
                return Ok(validtoken.AccessToken);
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
