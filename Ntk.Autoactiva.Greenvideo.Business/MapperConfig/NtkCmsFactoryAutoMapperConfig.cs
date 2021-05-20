using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;


namespace Ntk.Autoactiva.Greenvideo.Business.MapperConfig
{
    public static class NtkCmsFactoryAutoMapperConfig
    {
        public static NtkCmsFactoryAutoMapperConfigModule binder = new NtkCmsFactoryAutoMapperConfigModule();
        public static IMapper mapper;
        static NtkCmsFactoryAutoMapperConfig()
        {
            binder.cfg.AllowNullCollections = true;
            binder.cfg.AddExpressionMapping();
        }

        public static void Initialize()
        {

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(binder.cfg);
            mapper = new Mapper(mappingConfig);
        }
    }
}
