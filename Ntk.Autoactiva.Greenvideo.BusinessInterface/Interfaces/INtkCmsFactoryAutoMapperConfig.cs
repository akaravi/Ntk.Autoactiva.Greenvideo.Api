using AutoMapper;

namespace Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces
{
    public interface INtkCmsFactoryAutoMapperConfig
    {
        IMapper Mapper();
        void SetCreateMap<T1, T2>(bool ReverseMap = false);
        void SetCreateMapToMe<T>() where T : class;
        void SetCreateMapEntityAndViewModel<TEntity, TViewModel>() where TViewModel : class where TEntity : class;
    }
}
