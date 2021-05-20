
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;

namespace Ntk.Autoactiva.Greenvideo.Core.Config
{
    public class CoreIocConfig
    {
        public static INtkCmsFactoryIocConfig IocConfig { get; private set; }
        public static void Register(INtkCmsFactoryIocConfig ioc)
        {
            IocConfig = ioc;

           
        }
    }
}
