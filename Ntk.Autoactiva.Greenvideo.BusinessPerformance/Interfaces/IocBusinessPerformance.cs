namespace Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces
{
   public interface IocBusinessPerformance
    {
        IPerformanceLogTools GetInstancePerformanceLogTools();
        IPerformanceTaskManager GetInstancePerformanceTaskManager();
    }
}
