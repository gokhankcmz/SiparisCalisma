using System;
using System.Reflection;
using CommonLib.Helpers.Jwt;
using CommonLib.Middlewares;
using CustomerService.Filters;
using CustomerService.Utility;
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
using Repository;
using Serilog;

namespace CustomerService
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
            
            var mongoSettings = Configuration.GetSection("MongoSettings").Get<MongoSettings>();
            services.AddControllers()
                .AddFluentValidation(x =>
                {
                    x.DisableDataAnnotationsValidation = true;
                    x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                    x.ImplicitlyValidateChildProperties = true;
                });

            services.AddHealthChecks()
                .AddElasticsearch(Configuration.GetSection("ElasticConfiguration:Uri").Value, name: "Elastic Search",
                    tags: new[] {"logging", "service"}, timeout: TimeSpan.FromSeconds(5))
                .AddMongoDb(Configuration.GetSection("MongoSettings").Get<MongoSettings>().ConnectionString,
                    name: "MongoDb",
                    tags: new[] {"database", "mongo", "service"}, timeout: TimeSpan.FromSeconds(3));
            
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CustomerService", Version = "v1"});
            });
            services.AddMongo(Configuration, mongoSettings);
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddSingleton<IRepository<Customer>, Repository<Customer>>();
            services.AddSingleton<IApplicationService, ApplicationService>();
            services.AddScoped<ValidateCustomerExistAttribute>(); //Transient?
            services.AddScoped<ValidateEmailIsUniqueAttribute>(); //Transient?
            services.ConfigureJwt(Configuration);
            services.AddSingleton<AuthenticationManager<Customer>>();
            services.AddAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            
            app.UseErrorLogger();
            app.UseCustomErrorHandler();
            app.UseResponseManipulation();
            
            
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerService v1"));

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthorization();

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