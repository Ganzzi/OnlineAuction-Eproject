using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "UserName required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }

    }
}
