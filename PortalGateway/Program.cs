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
using System.Text;
using Microsoft.AspNetCore.Builder;

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
                   //var loader = new BlankAudienceInfoLoader();
                   //var jwtSection = loader.LoadAudienceInfo();
                   //s.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   //    .AddJwtBearer(options => {
                   //        options.TokenValidationParameters = new TokenValidationParameters {
                   //            ValidateIssuer = true,
                   //            ValidateAudience = true,
                   //            ValidateLifetime = true,
                   //            ClockSkew = TimeSpan.FromSeconds(3), //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                   //     ValidateIssuerSigningKey = true,
                   //            ValidAudience = jwtSection.Audience,
                   //            ValidIssuer = jwtSection.Issuer,
                   //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Secret))
                   //        };
                   //    });
                   s.AddOcelot().AddPolly();
               })
               .ConfigureLogging((hostingContext, logging) => {
                   //add your logging
               })
               .UseIISIntegration()
               .Configure(app => {
                   //app.UseAuthentication();
                   app.UseOcelot().Wait();
               })
               .Build()
               .Run();
        }




    }
}
