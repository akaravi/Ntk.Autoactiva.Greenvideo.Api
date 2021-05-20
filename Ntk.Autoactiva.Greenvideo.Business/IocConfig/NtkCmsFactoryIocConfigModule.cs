using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Model;
using StructureMap.Query;
using System;
using System.Collections.Generic;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;

namespace Ntk.Autoactiva.Greenvideo.Business.IocConfig
{
    public class NtkCmsFactoryIocConfigModule : INtkCmsFactoryIocConfig
    {
        private static ICmsConfiguration CmsConfig;
        public Dictionary<string, string> ConnectionStrings
        {
            get
            {
                return NtkCmsFactoryIocConfig.GetInstance<ICmsConfiguration>().ConnectionStrings;
            }
        }
        public ICmsConfiguration GetCmsConfiguration()
        {
            //return NtkCmsFactoryIocConfig.GetInstance<ICmsConfiguration>();
            if (CmsConfig == null)
                CmsConfig = new CmsConfiguration();

            return CmsConfig;
        }
        public void SetCmsConfiguration(ICmsConfiguration Model)
        {
            CmsConfig = Model;
        }
        public IEnumerable<InstanceRef> GetInstanceList<T>()
        {
            return NtkCmsFactoryIocConfig.GetInstanceList<T>();
        }
        public T GetInstance<T>()
        {
            return NtkCmsFactoryIocConfig.GetInstance<T>();
        }
        public object GetInstance(Type type)
        {
            return NtkCmsFactoryIocConfig.GetInstance(type);
        }
        public IT GetInstance<IT>(string Named)
        {
            return NtkCmsFactoryIocConfig.GetInstance<IT>(Named);
        }
        public IT GetInstance<IT>(string Named, int CountPartName)
        {
            return NtkCmsFactoryIocConfig.GetInstance<IT>(Named, CountPartName);
        }
        public bool GetInstanceStatusRegistery<T>()
        {
            return NtkCmsFactoryIocConfig.GetInstanceStatusRegistery<T>();
        }
        public bool GetInstanceStatusRegistery(Type type)
        {
            return NtkCmsFactoryIocConfig.GetInstanceStatusRegistery(type);
        }
        public bool GetInstanceStatusRegistery<IT>(string Named)
        {
            return NtkCmsFactoryIocConfig.GetInstanceStatusRegistery<IT>(Named);
        }




        public void UseClass<T, TChild>(string Named = "") where TChild : T
        {
            NtkCmsFactoryIocConfig.RegisterClass<T, TChild>(Named);
        }
        public void UseClassIWrapper<T, TChild>(string Named = "") where TChild : T
        {
            NtkCmsFactoryIocConfig.RegisterClassIWrapper<T, TChild>(Named);
        }
        public void UseClassSingleton<T, TChild>(string Named = "") where TChild : T
        {
            NtkCmsFactoryIocConfig.RegisterClassSingleton<T, TChild>(Named);
        }
        public void UseRenderClass<T, TChild>(string Named = "") where TChild : T
        {
            NtkCmsFactoryIocConfig.RegisterRenderClass<T, TChild>(Named);
        }
        public void UseRenderClass<T, TChild>(int paretNamed = 1) where TChild : T
        {
            NtkCmsFactoryIocConfig.RegisterRenderClass<T, TChild>(paretNamed);
        }
        public void UseRenderWidgetClass<T, TChild>(string Named = "") where TChild : T
        {
            NtkCmsFactoryIocConfig.UseRenderWidgetClass<T, TChild>(Named);
        }



        public IPerformanceLogTools GetInstancePerformanceLogTools()
        {
            return NtkCmsFactoryIocConfig.GetInstance<IPerformanceLogTools>();
        }

        public IPerformanceTaskManager GetInstancePerformanceTaskManager()
        {
            return NtkCmsFactoryIocConfig.GetInstance<IPerformanceTaskManager>();
        }


    }
}
