﻿using Application;
using AuctionOnline.SignalRHub;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .Build();

builder.Services.AddAppService(appSettings);  
builder.Services.AddInfrastructureServices(appSettings);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
                {
                    // x.JsonSerializerOptions.MaxDepth = 64; 
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    // x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    // không cần https để trao đổi metadata
    x.RequireHttpsMetadata = false;
    //save token vào AuthenticationProperties
    x.SaveToken = true;
    //định dạng chính xác việc xét jwt 
    x.TokenValidationParameters = new TokenValidationParameters
    {
        //Điều khiển việc xác thực SecurityKey đã ký securityToken. Nếu giá trị này là true, SecurityKey sẽ được kiểm tra
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings["IssuerSigningKey"])),
        //Điều khiển việc xác thực giá trị audience trong token. Nếu giá trị này là false, giá trị audience sẽ không được kiểm tra
        ValidateAudience = false,
        //Điều khiển việc xác thực giá trị issuer trong token. Nếu giá trị này là false, giá trị issuer sẽ không được kiểm tra
        ValidateIssuer = false,

        //kết hợp với Expires trong UserController để ra time chính xác 
        ClockSkew = TimeSpan.Zero,
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt => opt.AddPolicy(name: "mypolicy",
        policy =>
        {
            var origin = appSettings["ClientUrl"] ?? "*";
            policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(origin);
        }
    ));
//cloudinary 
builder.Services.Configure<CloudKey>(builder.Configuration.GetSection("CloudinarySetting"));

// signalR
builder.Services.AddSingleton<SharedDb>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("mypolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<AuctionHub>("/auctionHub");

app.Run();
