
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;

namespace Ntk.Autoactiva.Greenvideo.BusinessPerformance.Config
{
    public class BusinessPerformanceIocConfig
    {
        public static IocBusinessPerformance IocConfig { get; private set; }

        public static void Register(IocBusinessPerformance ioc)
        {
            IocConfig = ioc;
        }
    }
}
