using AutoMapper;
using AutoMapper.Configuration;
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;

namespace Ntk.Autoactiva.Greenvideo.Business.MapperConfig
{
    public class NtkCmsFactoryAutoMapperConfigModule : INtkCmsFactoryAutoMapperConfig
    {
        internal MapperConfigurationExpression cfg = new MapperConfigurationExpression();
        public IMapper Mapper()
        {
            return NtkCmsFactoryAutoMapperConfig.mapper;
        }
        public void SetCreateMap<T1, T2>(bool ReverseMap = false)
        {
            if (ReverseMap)
                cfg.CreateMap<T1, T2>().ReverseMap();
            else
                cfg.CreateMap<T1, T2>();
        }
        public void SetCreateMapToMe<T>() where T : class
        {
            cfg.CreateMap<T, T>();
        }
        public void SetCreateMapEntityAndViewModel<TEntity, TViewModel>()
        where TEntity : class
        where TViewModel : class
        {

            cfg.CreateMap<TEntity, TViewModel>().ReverseMap().MaxDepth(5);
        }

    }
}
