
using Application.DTO;
using Application.Interface;
using DomainLayer.Core;
using DomainLayer.Entities.helper;
using DomainLayer.Entities.Models;
using DomainLayer.SpecificationPattern;
using Infrastructure.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Service
{
    internal class ResetEmailService : IresetEmailService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _data;
        private readonly IUnitOfWork _u;
        public ResetEmailService(IConfiguration config, IUnitOfWork u, AppDbContext data)
        {
            _config = config;
            _u = u;
            _data = data;
        }

        public bool sendMail(EmailModel email)
        {
            var emailmess = new MimeMessage();
            var from = _config["EmailSetting:Mail"];
            var name = _config["EmailSetting:DisplayName"];
            emailmess.From.Add(new MailboxAddress(name, from));
            emailmess.To.Add(new MailboxAddress(email.To, email.To));
            emailmess.Subject = email.Subject;
            emailmess.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(email.Content)
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    //với cổng 587 => smtp.ethereal.email
                    //client.Connect(_config["EmailSetting:SmtpServe"], 587, SecureSocketOptions.StartTls);
                    client.Connect(_config["EmailSetting:Host"], 587, SecureSocketOptions.StartTls);
                    client.Authenticate(_config["EmailSetting:Mail"], _config["EmailSetting:Password"]);
                    client.Send(emailmess);
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }

        }
        //check email and Create token 
        public async Task<EmailModel> CheckEmailAndTokenEmail(string email)
        {
            try
            {
                var specUserEmail = new BaseSpecification<User>(x => x.Email == email);
                var UserEmail = await _u.Repository<User>().FindOne(specUserEmail);
                if (UserEmail == null)
                {
                    return null;
                }
                var tokenByte = RandomNumberGenerator.GetBytes(64);
                var emailToken = Convert.ToBase64String(tokenByte);
                UserEmail.tokenResetPassword = emailToken;
                UserEmail.ResetExpire = DateTime.Now.AddMinutes(15);
                await _u.SaveChangesAsync();
                var bodyemail = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));
                return bodyemail;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        // 
        public async Task<int> checkTokenEmailAndSaveNewPassword(ResetPasswordModel model)
        {
            try
            {
                if (model.ConfirmPassWord == null || model.PasswordReset == null || model.EmailToken == null || model.Email == null)
                {
                    return 0;
                }
                var newToken = model.EmailToken.Replace(" ", "+");
                var specUser = new BaseSpecification<User>(x => x.Email == model.Email);
                var user = await _u.Repository<User>().FindOne(specUser);
                if (newToken != model.EmailToken || user.ResetExpire < DateTime.Now)
                {
                    return -1;
                }
                user.Password = model.PasswordReset.ToString();
                _data.Attach(user);
                _data.Entry(user).State = EntityState.Modified;
                await _u.SaveChangesAsync();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        //
        public async Task<EmailModel> sendMailForSuccessBuyer(int buyerId)
        {
            try
            {
                var speccheckEmail = new BaseSpecification<User>(x => x.UserId == buyerId);
                var checkEmail = await _u.Repository<User>().FindOne(speccheckEmail);
                var bodyemail = new EmailModel(checkEmail.Email, "success Password", successMail.EmailForByer(checkEmail.Email));
                return bodyemail;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<EmailModel> sendMailForSuccessSeller(int sellerId)
        {
            try
            {
                var speccheckEmail = new BaseSpecification<User>(x => x.UserId == sellerId);
                var checkEmail = await _u.Repository<User>().FindOne(speccheckEmail);
                var bodyemail = new EmailModel(checkEmail.Email, "success Password", successMail.EmailForByer(checkEmail.Email));
                return bodyemail;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
