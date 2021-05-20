
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;

namespace Ntk.Autoactiva.Greenvideo.Config
{
    public class CoreMapperConfig
    {
        public static INtkCmsFactoryAutoMapperConfig MaperConfig { get; private set; }
        public static void Register(INtkCmsFactoryAutoMapperConfig maper)
        {
            MaperConfig = maper;


        }
    }
}
