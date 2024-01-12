using DomainLayer.Core;
using Infrastructure.Data;
using Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace Infrastructure
{
    public static class ConfigService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service, IConfiguration config)
        {
            service.AddDbContext<AppDbContext>(o =>
            {
                o.UseSqlServer(config["ConnectionStrings.DBConnectionString"],
                    x => x.MigrationsAssembly("Infrastructure"));
            });

            service.AddScoped(typeof(IBaseRepos<>), typeof(Repository<>));
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            return service;
        }
    }
}
