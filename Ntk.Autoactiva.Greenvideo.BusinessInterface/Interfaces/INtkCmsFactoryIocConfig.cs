using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;
using System;

namespace Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces
{
    public interface INtkCmsFactoryIocConfig :  IocBusinessPerformance
    {
        ICmsConfiguration GetCmsConfiguration();
        void SetCmsConfiguration(ICmsConfiguration Model);

        T GetInstance<T>();
        object GetInstance(Type type);
        IT GetInstance<IT>(string Named);
        IT GetInstance<IT>(string Named, int CountPartName);
        bool GetInstanceStatusRegistery<T>();
        bool GetInstanceStatusRegistery(Type type);
        bool GetInstanceStatusRegistery<IT>(string Named);

        void UseClass<T, T2>(string Named = "") where T2 : T;
        void UseClassIWrapper<T, T2>(string Named = "") where T2 : T;

        void UseClassSingleton<T, T2>(string Named = "") where T2 : T;
        void UseRenderClass<T, T2>(string Named = "") where T2 : T;
        void UseRenderClass<T, T2>(int paretNamed) where T2 : T;
        void UseRenderWidgetClass<T, T2>(string Named = "") where T2 : T;
    }
}
