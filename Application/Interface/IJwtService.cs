using DomainLayer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IJwtService
    {
        Task<string> CreateToken(User user);
        Task<RefreshToken> createRrefreshtoken(int? id);
        Task<RefreshToken> checkToken(string token);
    }
}
