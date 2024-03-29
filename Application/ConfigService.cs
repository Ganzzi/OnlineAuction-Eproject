﻿using Application.Interface;
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
            var cnts = config.GetConnectionString("RedisConnectionString");

            service.AddStackExchangeRedisCache(op => {
                op.Configuration = cnts;
            });
            
            service.AddSingleton<RedisService>();
            service.AddScoped<IJwtService, JwtService>();
            service.AddScoped<IAdminService, AdminService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IPhotoService, PhotoService>();
            service.AddScoped<IResetEmailService, ResetEmailService>();

            return service;
        }
    }
}

