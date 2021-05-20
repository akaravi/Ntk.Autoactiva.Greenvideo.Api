

using GermanyGreenVideo.BusinessDependency;
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;
using Ntk.Autoactiva.Greenvideo.Core.Config;

namespace GermanyGreenVideo.Config
{
    public class WebApiIocConfig
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
