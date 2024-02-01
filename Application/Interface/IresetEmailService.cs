using Application.DTO;
using DomainLayer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IresetEmailService
    {
        Task<EmailModel> CheckEmailAndTokenEmail(string email);
        bool sendMail(EmailModel email);
        Task<int> checkTokenEmailAndSaveNewPassword(ResetPasswordModel model);

        Task<EmailModel> sendMailForSuccessBuyer(int buyerId, int sellerId);
        Task<EmailModel> sendMailForSuccessSeller(int sellerId,int buyerId);
    }
}
