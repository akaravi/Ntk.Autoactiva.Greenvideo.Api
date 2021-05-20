using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using NLog.Web;
using Ntk.Autoactiva.Greenvideo.Business.IocConfig;
using Ntk.Autoactiva.Greenvideo.Business.MapperConfig;
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Model;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;
using Ntk.Autoactiva.Greenvideo.Worker.BusinessDependency;
using Ntk.Autoactiva.Greenvideo.Worker.Config;


namespace Ntk.Autoactiva.Greenvideo.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("Starting up the service");



                #region NtkCmsFactoryAutoMapperConfig
                WorkerMapperConfig.Register(NtkCmsFactoryAutoMapperConfig.binder);
                NtkCmsFactoryAutoMapperConfig.Initialize();
                #endregion NtkCmsFactoryAutoMapperConfig

                #region NtkCmsFactoryIocConfig
                WorkerIocConfig.Register(NtkCmsFactoryIocConfig.binder);
                NtkCmsFactoryIocConfig.Initilizer();
                var performanceLogTools = WorkerIocConfig.IocConfig.GetInstance<IPerformanceLogTools>();
                performanceLogTools = new PerformanceLogTools();


                var performanceTaskManager = WorkerIocConfig.IocConfig.GetInstance<IPerformanceTaskManager>();
                performanceTaskManager = new PerformanceTaskManager();

                #endregion NtkCmsFactoryIocConfig
                WorkerIocConfig.IocConfig.SetCmsConfiguration(appSettingRead());

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "There was a problem starting the serivce");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }

        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureServices((hostContext, services) =>
        //        {
        //            services.AddHostedService<Worker>();
        //        });
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ICmsConfiguration, CmsConfiguration>();
                    services.AddHostedService<Worker>();
                    //services.Configure<AppSettingsCCModel>(hostContext.Configuration.GetSection("AppSettings"));
                })
                //.ConfigureHostConfiguration(configHost =>
                //{
                //    configHost.SetBasePath(Directory.GetCurrentDirectory());
                //    configHost.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //})
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    //configApp.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                    //configApp.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    //logging .AddConsole();
                    //logging .AddDebug();
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
        }
        private static IConfiguration Config { get; set; }

        private static ICmsConfiguration appSettingRead()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Config = builder.Build();
            #region appsetting Read
            var cmsConfiguration = new CmsConfiguration();
            cmsConfiguration.VerConfig = "Wellecom ver: 1 Worker.WorkerService  Now:( " + DateTime.Now + " )";
            Config.Bind("AppSettings", cmsConfiguration.AppSettings);
            Config.Bind("ConnectionStrings", cmsConfiguration.ConnectionStrings);
            cmsConfiguration.AppMemory.WebRootPath = Config["Application:WebRootPath"];
            cmsConfiguration.AppMemory.ContentRootPath = Config["Application:ContentRootPath"];

            #endregion appsetting Read      

            return cmsConfiguration;
        }
    }
}
