using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;
using Ntk.Autoactiva.Greenvideo.Core.Config;
using Ntk.Autoactiva.Greenvideo.Worker.BusinessDependency;

namespace Ntk.Autoactiva.Greenvideo.Worker.Config
{
    public class WorkerIocConfig
    {

        public static INtkCmsFactoryIocConfig IocConfig { get; private set; }
        public static void Register(INtkCmsFactoryIocConfig ioc)
        {
            IocConfig = ioc;
            CoreIocConfig.Register(ioc);

            ioc.UseClassSingleton<IPerformanceTaskManager, PerformanceTaskManager>();
            ioc.UseClassSingleton<IPerformanceLogTools, PerformanceLogTools>();


        }
    }
}
