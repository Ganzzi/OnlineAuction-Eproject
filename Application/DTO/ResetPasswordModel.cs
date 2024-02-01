using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }
      
        public string EmailToken { get; set; }
        [Required(ErrorMessage = "PasswordReset required")]
        public string PasswordReset { get; set; }
        [Required(ErrorMessage = "ConfirmPassWord required")]
        public string ConfirmPassWord { get; set; }
    }
}
