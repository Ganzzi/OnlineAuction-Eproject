using Application.DTO;
using Application.Interface;
using DomainLayer.Core;
using DomainLayer.Entities.Models;
using DomainLayer.SpecificationPattern;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    internal class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unit;
    
        public AuthService(IUnitOfWork unit)
        {
            _unit = unit;     
        }
        public async Task<User> Login(LoginModel model)
        {
            //var spec2 = new BaseSpecification<User>
            //    (x => x.Name == model.UserName && x.Password == model.Password)
            //    .AddInclude(query => query.Include(user => user.bids).ThenInclude(bid => bid.Item));
            var spec = new BaseSpecification<User>(x => x.Password == model.Password && x.Name == model.UserName);

            var user = await _unit.Repository<User>().FindOne(spec);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<User> Register(RegisterModel model)
        {
            try
            {
                var newUser = new User()
                {
                    Name = model.Name,
                    Password = model.Password,
                    Email = model.Email
                };

                var spec = await _unit.Repository<User>().AddAsync(newUser);
                if (spec == null)
                {
                    return null;
                }
                await _unit.SaveChangesAsync();
                return spec;
            }
            catch (Exception ex)
            {
                await _unit.RollBackChangesAsync();
                return null;
            }
        }

      

        //public async Task<User> Update(string name, int id)
        //{
        //    var user = await _unit.Repository<User>().FindOne(new BaseSpecification<User>(x => x.UserId == id));
        //    user.Name = name;
        //    _unit.Repository<User>().Update(user);
        //    await _unit.SaveChangesAsync();
        //    return user;
        //}

     

    }
}
