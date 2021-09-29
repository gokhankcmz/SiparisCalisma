using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Kafka;

namespace Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureAppConfiguration((context, config) => config.AddJsonFile("ocelot.json"))
                .UseSerilog((context, configuration) =>
                {
                    configuration.Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .WriteTo.Console()
                        .WriteTo.Kafka(context.Configuration["Kafka:bootstrapServers"],
                            topic: context.Configuration["Kafka:topic"])
                        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProperty("Host", context.HostingEnvironment.ContentRootPath)
                        .ReadFrom.Configuration(context.Configuration);
                });

    }
}