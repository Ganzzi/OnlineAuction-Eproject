﻿using Application.Interface;
using DomainLayer.Core;
using DomainLayer.Entities.Models;
using DomainLayer.SpecificationPattern;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Service
{
    public class JwtService : IJwtService
    {
        private readonly IUnitOfWork _u;
        private readonly IConfiguration _config;

        public JwtService(IUnitOfWork u, IConfiguration config)
        {
            _u = u;
            _config = config;
        }
        public async Task<string> CreateToken(User user)
        {
            // JwtSecurityTokenHandler => cho phép thao tác với token(tạo - gọi method - xác thực)
            var jwtHandeler = new JwtSecurityTokenHandler();
            // tạo key
            var key = Encoding.ASCII.GetBytes(_config["IssuerSigningKey"]);
            // tạo playload chứa name và role
            var spec = new BaseSpecification<User>(x => x.Name == user.Name);
            var userRole = await _u.Repository<User>().FindOne(spec);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role,userRole.Role),
                new Claim(ClaimTypes.Name,$"{userRole.Name}")
            });
            //tạo chữ ký 
            var cerdential = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            // 
            var des = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = cerdential
            };
            var token = jwtHandeler.CreateToken(des);

            var AccessToken = jwtHandeler.WriteToken(token);
            Console.WriteLine(AccessToken);
            return AccessToken;
        }

        public async Task<RefreshToken> CreateRrefreshtoken(int? id)
        {
            var spec = new BaseSpecification<RefreshToken>(z => z.UserId == id);
            var existingToken = await _u.Repository<RefreshToken>().FindOne(spec);
            if (existingToken != null)
            {
                existingToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                existingToken.Created = DateTime.Now;
                existingToken.ExpiryDate = DateTime.Now.AddSeconds(70);
                _u.Repository<RefreshToken>().Update(existingToken);
                return existingToken;
            }
            var refreshtoken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                ExpiryDate = DateTime.Now.AddSeconds(700000),
                UserId = id
            };
            await _u.Repository<RefreshToken>().AddAsync(refreshtoken);
            await _u.SaveChangesAsync();
            return refreshtoken;
        }

        public async Task<RefreshToken> RefreshAccessToken(string token)
        {
            var unquiname = UsernameFormToken(token);
            var SpecUser = new BaseSpecification<User>(x => x.Name == unquiname);
            var user = await _u.Repository<User>().FindOne(SpecUser);

            var spec = new BaseSpecification<RefreshToken>(z => z.UserId == user.UserId);
            var refreshToken = await _u.Repository<RefreshToken>().FindOne(spec);
            if (refreshToken == null || refreshToken.ExpiryDate <= DateTime.Now)
            {
                return null;
            }
            else
            {
                var UserSpec = new BaseSpecification<User>(x => x.UserId == refreshToken.UserId);
                var User = await _u.Repository<User>().FindOne(UserSpec);
                var newRToken = await CreateRrefreshtoken(refreshToken.UserId);
                var newAccToken = await CreateToken(User);
                newRToken.AccessToken = newAccToken;
                return newRToken;
            }
        }

        public string UsernameFormToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtInput = token.Replace("Bearer ", string.Empty);
            var readableToken = jwtHandler.CanReadToken(jwtInput);
            if (!readableToken) return null;
            var decodedJwt = jwtHandler.ReadJwtToken(jwtInput);
            var claimName = decodedJwt.Claims;
            foreach (var item in claimName)
            {
                if (item.Type == "unique_name")
                {
                    return item.Value;
                }
            }
            return "";
        }


    }
}
