using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RiiZoo.Web.Auth;

namespace ConsumerService
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
            //读取配置文件
            var audienceConfig = new AudienceInfo();
            services.AddOcelotPolicyJwtBearer(audienceConfig.Issuer, 
                audienceConfig.Audience, audienceConfig.Secret, 
                "RiiZooBearer", "Permission", "no permission");
            //这个集合模拟用户权限表,可从数据库中查询出来
            var permission = new List<Permission> {
                              new Permission {  Url="/", RoleName="system"},
                              new Permission {  Url="/api/values", RoleName="system"}
                          };
            services.AddSingleton(permission);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
