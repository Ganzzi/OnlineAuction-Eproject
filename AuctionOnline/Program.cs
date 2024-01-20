using Application;
using DomainLayer.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//Add configServcie 
builder.Services.AddAppService(builder.Configuration);  
builder.Services.AddInfrastructureServices(builder.Configuration);

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

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my21charSuperSecretKeyForMy21charSuperSecretKey")),
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
            policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
        }
    ));
//cloudinary 
builder.Services.Configure<CloudKey>(builder.Configuration.GetSection("CloudinarySetting"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("mypolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
