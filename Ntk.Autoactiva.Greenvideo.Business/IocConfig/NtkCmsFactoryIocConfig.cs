using Microsoft.Extensions.DependencyInjection;
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Model;
using StructureMap;
using StructureMap.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ntk.Autoactiva.Greenvideo.Business.IocConfig
{
    public static class NtkCmsFactoryIocConfig
    {
        public static NtkCmsFactoryIocConfigModule binder = new NtkCmsFactoryIocConfigModule();
        private static int KeyObjectFactoryRegisterClass = 0;
        private static int KeyObjectFactoryRegisterDbContext = 0;
        private static int KeyObjectFactoryRegisterRenderWidgetClass = 0;
        private static readonly Lazy<Container> _containerBuilder = new Lazy<Container>(DefaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static Container Container_;
        public static IContainer Container
        {
            get
            {
                return _containerBuilder.Value;
            }
        }
        /// <summary>
        /// تمامی ماژول ها باید در این بخش معرفی شودند تا استراکچرمپ  شروع به کار کند
        /// </summary>
        /// <returns></returns>
        private static Container DefaultContainer()
        {
            return new Container(ioc =>
            {

            });
        }

        static NtkCmsFactoryIocConfig()
        {
            Container_ = new Container();
            //privateRegister<ICmsConfiguration, CmsConfiguration>(1);

            binder.UseClassSingleton<ICmsConfiguration, CmsConfiguration>();


            //BusinessPerformanceIocConfig.Register(binder);
            //HyperCoreIocConfig.Register(binder);


        }
        public static void Initilizer()
        {



        }

        public static IEnumerable<InstanceRef> GetInstanceList<T>()
        {
            return Container.Model.AllInstances;
        }

        public static T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }
        public static bool GetInstanceStatusRegistery<T>()
        {
            return Container.Model.AllInstances.Any(i => i.PluginType.Equals(typeof(T)));
        }
        public static object GetInstance(Type type)
        {
            return Container.GetInstance(type);
        }
        public static bool GetInstanceStatusRegistery(Type type)
        {
            return Container.Model.AllInstances.Any(i => i.PluginType.Equals(type));
        }
        public static IT GetInstance<IT>(string Named)
        {
            if (!string.IsNullOrEmpty(Named))
                Named = Named.ToLower();
            return Container.GetInstance<IT>(Named);

        }
        public static IT GetInstance<IT>(string Named, int CountPartName)
        {
            if (!string.IsNullOrEmpty(Named))
                Named = Named.ToLower();

            string[] NamedArray = Named.ToString().Split('.');
            if (CountPartName > NamedArray.Length)
                CountPartName = NamedArray.Length;
            Named = string.Join(".", NamedArray.Reverse().Take(CountPartName).Reverse());

            return Container.GetInstance<IT>(Named);

        }

        public static bool GetInstanceStatusRegistery<IT>(string Named)
        {
            if (!string.IsNullOrEmpty(Named))
                Named = Named.ToLower();
            return Container.Model.AllInstances.Any(i => i.Name.Equals(Named));
        }






        public static void ClearDatabaseInitilizer()
        {
            //WebDesignerIocConfig.ClearDatabaseInitilizer();

        }
        public static void UpdateDatabaseInitilizer()
        {
            //WebDesignerIocConfig.UpdateDatabaseInitilizer();

        }





        //PerRequest = 0, در برنامه‌های وب کاربرد دارد (و حالت پیش فرض است). با انتخاب آن وهله سازی کلاس مورد نظر به ازای هر درخواست رسیده انجام خواهد شد.
        //Singleton = 1,کلاس به صورت Singleton عمل کند،
        //ThreadLocal = 2, به ازای هر Thread، وهله‌ای متفاوت در اختیار مصرف کننده قرار می‌گیرد.
        //HttpContext = 3,به ازای هر HttpContext ایجاد شده، کلاس معرفی شده یکبار وهله سازی می‌گردد.
        //Hybrid = 4,رکیبی است از حالت‌های HttpContext و ThreadLocal. اگر برنامه وب بود، از HttpContext استفاده خواهد کرد در غیراینصورت به ThreadLocal سوئیچ می‌کند.
        //HttpSession = 5,
        //HybridHttpSession = 6,
        //Unique = 7,
        //Transient = 8,
        //
        //در برنامه‌های دسکتاپ خود اگر از ترد استفاده نمی‌کنید، از حالت HybridHttpOrThreadLocalScoped برای مدیریت طول عمر UOW استفاده نکنید.
        // HybridHttpSession امین روی این گذاشته بود
        // در برنامه‌هاي دسكتاپ از حالت هيبريد استفاده نكنيد
        //Singleton()
        //HttpContextScoped()  //تا 951023 روی این حالت بود
        //HybridHttpOrThreadLocalScoped()


        /// <summary>
        /// PerRequest=0
        /// Singleton=1
        /// ThreadLocal=2
        /// Hybrid=3
        /// HttpSession=4
        /// HybridHttpSession=5
        /// Unique=6
        /// Transient=7
        /// </summary>
        public enum InstanceScope
        {
            PerRequest = 0,
            Singleton = 1,
            ThreadLocal = 2,
            HttpContext = 3,
            Hybrid = 4,
            HttpSession = 5,
            HybridHttpSession = 6,
            Unique = 7,
            Transient = 8,
        }

        public static void RegisterRenderClass<T, TViewModel>(string Named = "") where TViewModel : T
        {
            if (string.IsNullOrEmpty(Named))
                Named = typeof(TViewModel).ToString().Split('.').LastOrDefault();

            privateRegister<T, TViewModel>(KeyObjectFactoryRegisterClass, Named);
        }

        public static void RegisterClassIWrapper<T, TViewModel>(string Named = "") where TViewModel : T
        {
            if (string.IsNullOrEmpty(Named))
                Named = typeof(TViewModel).ToString().Split('.').LastOrDefault();

            privateRegister<T, TViewModel>(KeyObjectFactoryRegisterClass, Named);
        }

        public static void RegisterRenderClass<T, TChild>(int paretNamed) where TChild : T
        {
            string[] NamedArray = typeof(TChild).ToString().Split('.');
            if (paretNamed > NamedArray.Length)
                paretNamed = NamedArray.Length;
            string Named = string.Join(".", NamedArray.Reverse().Take(paretNamed).Reverse());


            privateRegister<T, TChild>(KeyObjectFactoryRegisterClass, Named);
        }
        public static void UseRenderWidgetClass<T, TChild>(string Named = "") where TChild : T
        {
            if (string.IsNullOrEmpty(Named))
                Named = typeof(TChild).ToString().Split('.').LastOrDefault();

            privateRegister<T, TChild>(KeyObjectFactoryRegisterRenderWidgetClass, Named);
        }
        public static void RegisterClass<T, TChild>(string Named = "") where TChild : T
        {
            privateRegister<T, TChild>(KeyObjectFactoryRegisterClass, Named);
        }
        public static void RegisterClassSingleton<T, TChild>(string Named = "") where TChild : T
        {
            privateRegister<T, TChild>(1, Named);
        }

        public static void RegisterDbContext<T, TChild>() where TChild : T
        {
            privateRegister<T, TChild>(KeyObjectFactoryRegisterDbContext);
        }

        public static void RegisterConfig(Action<ConfigurationExpression> configure)
        {
            Container.Configure(configure);
        }
        public static void RegisterConfigServiceCollection(IServiceCollection serviceCollection)
        {
            Container.Populate(serviceCollection);
            //Container. Configure(x => { x.Populate(serviceDescriptors); });
        }
        //public static Container Configure(IServiceCollection services)
        //{
        //    var container = new Container(config =>
        //    {
        //        config.Scan(s =>
        //        {
        //            s.TheCallingAssembly();
        //            s.WithDefaultConventions();
        //            s.AddAllTypesOf<IStartupTask>();
        //            s.LookForRegistries();
        //            s.AssembliesAndExecutablesFromApplicationBaseDirectory();
        //        });
        //        config.Populate(services);
        //    });

        //    return container;
        //}
        private static void privateRegister<T, TChild>(int Method, string Named = "") where TChild : T
        {
            if (!string.IsNullOrEmpty(Named))
                Named = Named.ToLower();
            Container.Configure(ioc =>
            {
                switch (Method)
                {
                    default:
                    case 0: //PerRequest
                        if (!string.IsNullOrEmpty(Named))
                            ioc.For<T>().Use<TChild>().Named(Named);
                        else
                            ioc.For<T>().Use<TChild>();
                        break;
                    case 1: //Singleton
                            // مثل حالت 
                            //static
                            //عمل می کند
                        if (!string.IsNullOrEmpty(Named))
                            ioc.For<T>().Singleton().Use<TChild>().Named(Named);
                        else
                            ioc.For<T>().Singleton().Use<TChild>();
                        break;
                    case 2: //ThreadLocal
                        // وقتی که یک ترد ران میشود
                        // تا باز بودن ترد یکی می دهد
                        if (!string.IsNullOrEmpty(Named))
                            ioc.For<T>().Use<TChild>().Named(Named).SetLifecycleTo(StructureMap.Pipeline.Lifecycles.ThreadLocal);
                        else
                            ioc.For<T>().Use<TChild>().SetLifecycleTo(StructureMap.Pipeline.Lifecycles.ThreadLocal);
                        break;
                    //case 3: //Hybrid
                    //    if (!string.IsNullOrEmpty(Named))
                    //        ioc.For<T>().HybridHttpOrThreadLocalScoped().Use<TChild>().Named(Named);
                    //    else
                    //        ioc.For<T>().HybridHttpOrThreadLocalScoped().Use<TChild>();
                    //    break;
                    //case 4: //HttpSession
                    //    if (!string.IsNullOrEmpty(Named))
                    //        ioc.For<T>().HttpContextScoped().Use<TChild>().Named(Named);
                    //    else
                    //        ioc.For<T>().HttpContextScoped().Use<TChild>();
                    //    break;
                    //case 5: //HybridHttpSession          
                    //    if (!string.IsNullOrEmpty(Named))
                    //        ioc.For<T>().HybridHttpOrThreadLocalScoped().Use<TChild>().Named(Named);
                    //    else
                    //        ioc.For<T>().HybridHttpOrThreadLocalScoped().Use<TChild>();
                    //    break;
                    case 6: //Unique
                        if (!string.IsNullOrEmpty(Named))
                            ioc.For<T>().Use<TChild>().Named(Named).SetLifecycleTo(StructureMap.Pipeline.Lifecycles.Unique);
                        else
                            ioc.For<T>().Use<TChild>().SetLifecycleTo(StructureMap.Pipeline.Lifecycles.Unique);
                        break;
                    case 7: //Transient
                        if (!string.IsNullOrEmpty(Named))
                            ioc.For<T>().Use<TChild>().Named(Named).SetLifecycleTo(StructureMap.Pipeline.Lifecycles.Transient);
                        else
                            ioc.For<T>().Use<TChild>().SetLifecycleTo(StructureMap.Pipeline.Lifecycles.Transient);
                        break;
                    case 8:
                        break;
                }
            });

        }
    }
}