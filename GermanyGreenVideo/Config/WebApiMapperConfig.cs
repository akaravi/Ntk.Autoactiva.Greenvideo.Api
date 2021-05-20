

using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;
using Ntk.Autoactiva.Greenvideo.Config;

namespace GermanyGreenVideo.Config
{
    public class WebApiMapperConfig
    {
        public static INtkCmsFactoryAutoMapperConfig MaperConfig { get; private set; }
        public static void Register(INtkCmsFactoryAutoMapperConfig maper)
        {
            MaperConfig = maper;
            CoreMapperConfig.Register(maper);

            //ProcessApplicationMapperConfig.Register(maper);
            //ModulesRelationshipMapperConfig.Register(maper);
            //WebApiControllerMapperConfig.Register(maper);

        }
    }
}
