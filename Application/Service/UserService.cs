using DomainLayer.Core;
using DomainLayer.Entities.Models;
using DomainLayer.SpecificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    internal class UserService
    {
        private readonly IUnitOfWork _u;
        public UserService(IUnitOfWork u)
        {
            _u = u;
        }


        //UserProfile ****
        public async Task<User> getUser(string username)
        {
            var spec = new BaseSpecification<User>(x => x.Name == username);
            var User = await _u.Repository<User>().FindOne(spec);
            try
            {
                if (User == null)
                {
                    return null;
                }
                else
                {
                    return User;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // update User
        public async Task<User> UpdateUser(User model)
        {
            try
            {
                var Userspec = new BaseSpecification<User>(x => x.Name == model.Name);
                var User = await _u.Repository<User>().FindOne(Userspec);
                if (User != null)
                {
                    User.Name = model.Name;
                    User.Email = model.Email;
                    User.Password = model.Password;
                    await _u.SaveChangesAsync();
                    return User;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                await _u.RollBackChangesAsync();
                return null;
            }

        }
    }
}
