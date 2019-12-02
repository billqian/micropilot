using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

namespace PortalGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) => {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                       .AddJsonFile("ocelot.json")
                       .AddEnvironmentVariables();
               })
               .ConfigureServices(s => {
                   s.AddOcelot().AddPolly();
               })
               .ConfigureLogging((hostingContext, logging) => {
                   //add your logging
               })
               .UseIISIntegration()
               .Configure(app => {
                   app.UseOcelot().Wait();
               })
               .Build()
               .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder
                    .ConfigureAppConfiguration(conbuilder => {
                        conbuilder.AddJsonFile("ocelot.json");
                    })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>().UseUrls("http://*:5000");
                });

        
    }
}
