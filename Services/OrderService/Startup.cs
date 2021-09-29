using System;
using System.Collections.Generic;
using System.Reflection;
using CommonLib.Helpers.Jwt;
using CommonLib.Middlewares;
using Confluent.Kafka;
using Entities.Models;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OrderService.Filters;
using OrderService.Utility;
using Repository;
using Serilog;

namespace OrderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(x =>
                {
                    x.DisableDataAnnotationsValidation = true;
                    x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                    x.ImplicitlyValidateChildProperties = true;
                });

            services.AddSwagger();
            services.AddMongo(Configuration);
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddSingleton<IRepository<Order>, Repository<Order>>();
            services.AddScoped<ValidateOrderExistAttribute>();
            services.AddScoped<ValidateCustomerExistAttribute>();
            services.AddSingleton<IApplicationService, ApplicationService>();
            services.ConfigureJwt(Configuration);

            services.AddHealthChecks()
                .AddElasticsearch(Configuration.GetSection("ElasticConfiguration:Uri").Value, name: "Elastic Search",
                    tags: new[] {"logging", "service"}, timeout: TimeSpan.FromSeconds(5))
                .AddMongoDb(Configuration.GetSection("MongoSettings").Get<MongoSettings>().ConnectionString,
                    name: "MongoDb",
                    tags: new[] {"database", "mongo", "service"}, timeout: TimeSpan.FromSeconds(3))
                .AddCheck("Customer Service", new CustomerServiceHealthCheck())
                .AddKafka(config =>
                {
                    config.BootstrapServers = Configuration["Kafka:bootstrapServers"];
                }, name: "Kafka", tags: new[] {"logging", "kafka"});


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }    
            
            
            app.UseCustomErrorHandler();
            app.UseResponseManipulation();
            
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService v1"));
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCustomerTokenValidation();
            
            app.UseSerilogRequestLogging();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/quickhealth", new HealthCheckOptions()
                {
                    Predicate = _ => false
                });
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}