using Application.Interface;
using Application.Service;
using Application.Service.AdminServicevice;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ConfigService
    {
        public static IServiceCollection AddAppService(this IServiceCollection service, IConfiguration config)
        {
            service.AddScoped<IJwtService, JwtService>();
            service.AddScoped<IAdminServicevice, AdminService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IuserService, UserService>();
            service.AddScoped<IphotoService, PhotoService>();
            service.AddScoped<IresetEmailService, ResetEmailService>();
            return service;
        }
    }
}

