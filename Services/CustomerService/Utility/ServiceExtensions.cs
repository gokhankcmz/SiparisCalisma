﻿using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Repository;

namespace CustomerService.Utility
{
    public static class ServiceExtensions
    {
        public static void AddMongo(this IServiceCollection services, IConfiguration configuration, MongoSettings mongoSettings)
        {
            services.AddSingleton(mongoSettings);
            services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));
            services.AddSingleton<IRepository<Order>, Repository<Order>>();
        }
    }
}