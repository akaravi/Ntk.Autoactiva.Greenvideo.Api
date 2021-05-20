using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;

namespace GermanyGreenVideo.BusinessDependency
{
    public static class PerformanceLogStartup
    {
        internal static void ConfigureServices(IServiceCollection services, ICmsConfiguration cmsConfiguration)
        {
            if (cmsConfiguration.AppSettings.Elmah.Status)
                services.AddElmah<XmlFileErrorLog>(options =>
                {
                    options.ApplicationName = "Ntk MVC Cms Error Log";
                    options.OnPermissionCheck = context => true;
                    options.Path = cmsConfiguration.AppSettings.Elmah.ElmahPath;// "elmah";
                    options.LogPath = cmsConfiguration.AppSettings.Elmah.ElmahLogPath;// "~/ElmahLog";  
                                                                                      //options.FiltersConfig = "elmah.xml";
                    options.Filters.Add(new MyFilter());

                });
        }
        internal static void ConfigureRegister(IApplicationBuilder app, IWebHostEnvironment env, ICmsConfiguration cmsConfiguration)
        {
            app.UseStatusCodePages();
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            if (cmsConfiguration.AppSettings.Elmah.Status)
            {
                app.UseWhen(
                    context => context.Request.Path.StartsWithSegments("/elmah", StringComparison.OrdinalIgnoreCase),
                    appBuilder =>
                    {
                        appBuilder.Use(next =>
                        {
                            return async ctx =>
                            {
                                ctx.Features.Get<IHttpBodyControlFeature>().AllowSynchronousIO = true;

                                await next(ctx);
                            };
                        });
                    });
                app.UseElmah();
            }

        }
    }

    internal class MyFilter : IErrorFilter
    {
        public void OnErrorModuleFiltering(object sender, ExceptionFilterEventArgs args)
        {
            //var aaaa = ((ElmahCore.HttpException)args.Exception).StatusCode;
        }
    }
}
