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

        //UserProfile
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

        //public async Task<> UpdateUser(int id)
        //{
        //    var spec = new BaseSpecification<User>()
        //}
    }
}
