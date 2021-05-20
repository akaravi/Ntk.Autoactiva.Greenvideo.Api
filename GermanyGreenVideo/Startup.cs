using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GermanyGreenVideo.BusinessDependency;
using GermanyGreenVideo.Config;
using GermanyGreenVideo.Util.Attributed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ntk.Autoactiva.Greenvideo.Business.IocConfig;
using Ntk.Autoactiva.Greenvideo.Business.MapperConfig;
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;

namespace GermanyGreenVideo
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            var configMemory = new Dictionary<String, String>();
            configMemory.Add("Application:Path", env.ContentRootPath);
            configMemory.Add("Application:ContentRootPath", env.ContentRootPath);
            configMemory.Add("Application:WebRootPath", env.WebRootPath);
            configMemory.Add("Application:Env", env.EnvironmentName);

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables("ASPNETCORE_")
                .AddInMemoryCollection(configMemory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            // Auto Mapper Configurations
            #region NtkCmsFactoryAutoMapperConfig
            WebApiMapperConfig.Register(NtkCmsFactoryAutoMapperConfig.binder);
            NtkCmsFactoryAutoMapperConfig.Initialize();
            #endregion NtkCmsFactoryAutoMapperConfig
        }

        public IConfiguration Configuration { get; }
        private ICmsConfiguration cmsConfiguration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region NtkCmsFactoryIocConfig
            WebApiIocConfig.Register(NtkCmsFactoryIocConfig.binder);
            NtkCmsFactoryIocConfig.RegisterConfigServiceCollection(services);
            NtkCmsFactoryIocConfig.Initilizer();
            #endregion NtkCmsFactoryIocConfig


            cmsConfiguration = appSettingRead();
            PerformanceLogStartup.ConfigureServices(services, cmsConfiguration);

            services.AddCors(o => o.AddPolicy("Policy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GermanyGreenVideo v1",
                    Version = "V1",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<UploadPartAttribute>();
            services.AddScoped<UploadCompeleteAttribute>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GermanyGreenVideo v1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Policy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseFileServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Web Api Didn't Find AnyThing :(");
            });
        }
        private ICmsConfiguration appSettingRead(bool TemplateInwwwroot = true)
        {
            #region appsetting Read
            var cmsConfiguration = WebApiIocConfig.IocConfig.GetCmsConfiguration();
            cmsConfiguration.VerConfig = "Wellecom ver: 1 Mvc Core Now:( " + DateTime.Now + " )";
            Configuration.Bind("AppSettings", cmsConfiguration.AppSettings);
            Configuration.Bind("ConnectionStrings", cmsConfiguration.ConnectionStrings);
            cmsConfiguration.AppMemory.WebRootPath = Configuration["Application:WebRootPath"];
            cmsConfiguration.AppMemory.ContentRootPath = Configuration["Application:ContentRootPath"];

            #endregion appsetting Read      

            return cmsConfiguration;
        }
    }
}
