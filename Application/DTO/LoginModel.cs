using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }    
    }
}
