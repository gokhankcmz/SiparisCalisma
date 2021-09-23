using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Repository;

namespace OrderService.Utility
{
    public static class ServiceExtensions
    {
        public static void AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoSettings").Get<MongoSettings>();
            services.AddSingleton(mongoSettings);
            services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));
            services.AddSingleton<IRepository<Order>, Repository<Order>>();
        }
    }
}