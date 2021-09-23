using System.Reflection;
using CommonLib.Middlewares;
using Entities.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OrderService.Filters;
using OrderService.Utility;
using Repository;

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "OrderService", Version = "v1"});
            });
            services.AddMongo(Configuration);
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddSingleton<IRepository<Order>, Repository<Order>>();
            services.AddScoped<ValidateOrderExistAttribute>();
            services.AddScoped<ValidateCustomerExistAttribute>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCustomErrorHandler();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService v1"));
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}