using Application.DTO;
using DomainLayer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAuthService
    {
        Task<User> Login(LoginModel model);

        Task<User> Register (RegisterModel model);

        string HashPassWord(string password);
    }
}
