using Application.DTO;
using DomainLayer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IResetEmailService
    {
        Task<EmailModel> CheckEmailAndTokenEmail(string email);
        bool SendMail(EmailModel email);
        Task<int> CheckTokenEmailAndSaveNewPassword(ResetPasswordModel model);

        Task<EmailModel> SendMailForSuccessBuyer(int buyerId, int sellerId);
        Task<EmailModel> SendMailForSuccessSeller(int sellerId,int buyerId);
    }
}
